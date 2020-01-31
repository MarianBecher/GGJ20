using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] 
    private float moveSpeed;
    
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

        //_rigid.AddForce(input * moveSpeed, ForceMode2D.Impulse);
    }
}
