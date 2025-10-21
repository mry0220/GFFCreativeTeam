using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire : MonoBehaviour
{
    [Header("右への速度"), SerializeField] private float _horizontalSpeed = 25;  // 右への移動速度

    private Rigidbody _rb;

    private Vector2 _moveVector;
    private Vector2 _inputVector;

    private bool _isGrounded;

    private float _FallTime = 0f;
    private float _fallSpeed = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rb.velocity;
        velocity.x = _horizontalSpeed;

        if(!_isGrounded )
        {
            _FallTime += Time.deltaTime;

            _fallSpeed = Physics.gravity.y * _FallTime * 3f * 3f;

            velocity.y += _fallSpeed * Time.fixedDeltaTime;
        }

            _rb.velocity = velocity;
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
