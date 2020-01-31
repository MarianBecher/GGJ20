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
        //Pickup
        //Drop
        //Place
        //Build
    }
}
