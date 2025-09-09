using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{



#if false
    private Rigidbody _rb;

    private Vector2 _moveVector;

    private bool _isGrounded;
    private bool _isJump;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    float _FallTime;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(_isJump);
            _isJump = true;
        }

    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            _moveVector.x = Input.GetAxisRaw("Horizontal");

            if (_isJump)
            {
               _rb.AddForce(0f,_jumpPower,0f,ForceMode.Impulse);
                _isJump=false;
            }
            
        }
        else
        {
            _FallTime += Time.deltaTime;

            _moveVector.y = Physics.gravity.y * _FallTime * 0.1f; 
        }
            _rb.velocity = _moveSpeed * _moveVector;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _FallTime = 0;
            _isGrounded = false;
        }
    }
#endif
}
