using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] 
    private float acceleration = 0.5f;
    [SerializeField]
    private float maxSpeed = 1;

    private Rigidbody2D _rigid;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void Move(Vector2 input)
    {
        if (input.magnitude > 1)
            input.Normalize();

        _rigid.AddForce(input * acceleration, ForceMode2D.Impulse);

        if(_rigid.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            _rigid.velocity = _rigid.velocity.normalized * maxSpeed;
        }

        if(input.magnitude < 0.1f)
        {
            _rigid.velocity = Vector2.zero;
        }
    }
}
