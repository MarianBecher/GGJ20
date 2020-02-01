using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Character : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] 
    private float acceleration = 0.5f;
    [SerializeField]
    private float maxSpeed = 1;
    public bool disableMovement = false;

    [Header("Components")]
    [SerializeField]
    private Transform _rotatingBody;
    [SerializeField]
    private Transform _itemContainer;
    private Rigidbody2D _rigid;
    private Animator _animator;


    private Item _currentItem;
    private Interactable _possibleInteractable;
    private List<GameObject> _currentObstacles = new List<GameObject>();
    private bool FreeSpaceInFront => _currentObstacles.Count == 0;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    
    public void Move(Vector2 input)
    {
        if (input.magnitude > 1)
            input.Normalize();


        bool moving = true;
        if (disableMovement || input.magnitude < 0.1f)
        {
            moving = false;
            _rigid.velocity = Vector2.zero;
        }
        else
        {
            _rigid.AddForce(input * acceleration, ForceMode2D.Impulse);

            if (_rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                _rigid.velocity = _rigid.velocity.normalized * maxSpeed;
                float angle = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;
                _rotatingBody.localRotation = Quaternion.Euler(0,0, angle);
                _animator.SetFloat("xMovement", _rigid.velocity.x);
                _animator.SetFloat("yMovement", _rigid.velocity.y);
            }
        }

        _animator.SetBool("Moving", moving);
    }

    public void Interact()
    {
        bool actionDone = false;
        if (!actionDone) { actionDone = _TryInteractWithHolder(); }
        if (!actionDone) { actionDone = _TryInteractWithWorkBench(); }
        if (!actionDone) { actionDone = _TryPickupItem(); }
        if (!actionDone) { actionDone = _TryDropItem(); }
        //Pickup
        //Drop
        //Place
        //Build
    }

    public void SubmitQTEAction(int btnIdx)
    {
        if (!_possibleInteractable || !(_possibleInteractable is WorkingBench))
            return;

        WorkingBench bench = _possibleInteractable as WorkingBench;
        if(bench.InQTE)
        {
            bench.SubmitQTE(btnIdx);
        }
    }

    private bool _TryPickupItem()
    {
        if (_currentItem || !_possibleInteractable || !(_possibleInteractable is Item))
            return false;

        Item itm = _possibleInteractable as Item;
        itm.Pickup();
        _HoldItem(itm);
        _possibleInteractable = null;
        return true;
    }

    private bool _TryDropItem()
    {
        if (!_currentItem || !FreeSpaceInFront)
            return false;

        _currentItem.Drop();
        _currentItem.transform.SetParent(null);
        _currentItem = null;
        return true;
    }

    private bool _TryInteractWithWorkBench()
    {
        if(!_possibleInteractable || !_currentItem || !(_possibleInteractable is WorkingBench))
            return false;

        WorkingBench bench = _possibleInteractable as WorkingBench; 
        if(bench.Interact(_currentItem, this))
        {
            _currentItem = null;
        }

        return true;
    }

    private bool _TryInteractWithHolder()
    {
        if (!_possibleInteractable || !(_possibleInteractable is ItemHolder))
            return false;

        ItemHolder holder = _possibleInteractable as ItemHolder;

        //Put Item
        if (_currentItem && !holder.HasItem())
        {
            if(holder.PutItem(_currentItem))
            {
                _currentItem = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get Item
        if (!_currentItem && holder.HasItem())
        {
            _HoldItem(holder.GetItem());
        }

        return false;
    }

    private void _HoldItem(Item itm)
    {
        _currentItem = itm;
        _currentItem.transform.SetParent(_itemContainer);
        _currentItem.transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if(interactable)
            {
                _possibleInteractable = interactable;
                _possibleInteractable.EnableHighlight();
            }
        }

        if (collision.gameObject.CompareTag("Obstacle") && !_currentObstacles.Contains(collision.gameObject))
            _currentObstacles.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.DisableHighlight();
            if (_possibleInteractable && other.gameObject == _possibleInteractable.gameObject)
            {
                _possibleInteractable = null;
            }
        }

        if (other.gameObject.CompareTag("Obstacle") && _currentObstacles.Contains(other.gameObject))
            _currentObstacles.Remove(other.gameObject);
    }
}
