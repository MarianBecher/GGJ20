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
    private Item _possibleItemToSelect;
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
            }
        }

        _animator.SetBool("Moving", moving);
        _animator.SetFloat("xMovement", _rigid.velocity.x);
        _animator.SetFloat("yMovement", _rigid.velocity.y);
    }

    public void Interact()
    {
        Debug.Log("interact");

        bool actionDone = false;
        if (!actionDone) { actionDone = _TryPickupItem(); }
        if (!actionDone) { actionDone = _TryDropItem(); }
        //Pickup
        //Drop
        //Place
        //Build
    }

    private bool _TryPickupItem()
    {
        if (!_currentItem && _possibleItemToSelect)
        {
            _possibleItemToSelect.Pickup();
            _currentItem = _possibleItemToSelect;
            _currentItem.transform.SetParent(_itemContainer);
            _currentItem.transform.localPosition = Vector3.zero;
            _possibleItemToSelect = null;
            return true;
        }

        return false;
    }

    private bool _TryDropItem()
    {
        if (_currentItem && FreeSpaceInFront)
        {
            _currentItem.Drop();
            _currentItem.transform.SetParent(null);
            _currentItem = null;
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Interactable"))
        {
            if(_currentItem)
            {
            }
            else
            {
                //Check if we can pickup an item
                Item itm = collision.GetComponent<Item>();
                if (itm != null)
                {
                    _possibleItemToSelect = itm;
                }
            }
        }
        _obstacleCount++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(_possibleItemToSelect != null && other.gameObject == _possibleItemToSelect.gameObject)
        {
            _possibleItemToSelect = null;
        }
        _obstacleCount--;
    }
}
