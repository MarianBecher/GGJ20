using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Character : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] 
    private float acceleration = 0.5f;
    [SerializeField]
    private float maxSpeed = 1;

    [Header("Components")]
    [SerializeField]
    private Transform _rotatingBody;
    [SerializeField]
    private Transform _itemContainer;
    private Rigidbody2D _rigid;
    private Animator _animator;


    private Item _currentItem;
    private Interactable _possibleInteractable;
    private int _obstacleCount = 0;
    private bool FreeSpaceInFront => _obstacleCount == 0;

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
        if (input.magnitude < 0.1f)
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
        if(bench.Interact(_currentItem))
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
        _obstacleCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(_possibleInteractable != null && other.gameObject == _possibleInteractable.gameObject)
        {
            _possibleInteractable = null;
        }
        _obstacleCount--;
    }
}
