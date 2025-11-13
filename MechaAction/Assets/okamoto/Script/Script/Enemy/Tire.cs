using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire : MonoBehaviour
{
    private float _moveSpeed = 5f;  // ‰E‚Ö‚ÌˆÚ“®‘¬“x

    private Rigidbody _rb;

    private bool _isGrounded;

    private float _FallTime = 0f;
    private float _fallSpeed = 1f;
    private int dir;
    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //velocity = _rb.velocity;

        //velocity.x = _moveSpeed * dir;

        //if(!_isGrounded )
        //{
        //    _FallTime += Time.deltaTime;

        //    _fallSpeed = Physics.gravity.y * _FallTime * 2f * 2f;

        //    velocity.y += _fallSpeed * Time.fixedDeltaTime;
        //}

        //    _rb.velocity = velocity;
    }

    public void _FallStart(int _dir)
    {
        _FallTime = 0;
        dir = _dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _FallTime = 0;
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
}
