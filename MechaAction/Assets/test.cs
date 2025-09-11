using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class test : MonoBehaviour
{
    private Rigidbody _rb;

    private Vector2 _moveVector;

    private Vector2 _inputVector;

    private bool _isGrounded;
    private bool _isJump;
    private bool _isCooltime;

    private float _cooldownTime = 3;
    private float _nextAttackTime = 0;

    private WaitForSeconds _sleepTime =new(3); 

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
        if (Input.GetMouseButton(0) && !_isCooltime)
        {
            Attack();
        }

        //cooldown();

    }

    private void FixedUpdate()
    {
        
            _inputVector.x = Input.GetAxisRaw("Horizontal");
            _inputVector.y = Input.GetAxisRaw("Vertical");
        Command(_inputVector);
        if (_isGrounded)
        {
            _moveVector.y = 0f; //yé≤ÇÃèâä˙âª
            
            _moveVector.x = _inputVector.x;
            if (_isJump)
            {
               _rb.AddForce(0f,_jumpPower,0f,ForceMode.Impulse);
                _isJump=false;
            }
            
        }
        else
        {
            _FallTime += Time.deltaTime;

            _moveVector.y = Physics.gravity.y * _FallTime * 0.5f; 
        }
            _rb.velocity = _moveSpeed * _moveVector;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _FallTime = 0;
            _isGrounded = true;
            Debug.Log("hoge");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _FallTime = 0;
            _isGrounded = false;
            Debug.Log("fuga");
        }
    }

    /*private void cooldown()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Time.time >= _nextAttackTime)
            {
                Attack();
                _nextAttackTime = Time.time + _cooldownTime;
            }
        }
    }*/

    private void Attack()
    {
        
        Debug.Log("çUåÇÇµÇΩ");
        StartCoroutine(Cooltime());
    }

    private IEnumerator Cooltime()
    {
        _isCooltime = true;
        yield return _sleepTime;
        _isCooltime = false;
    }

    private void Command(Vector2 InputVector)
    {
        if (InputVector.x == 0 && InputVector.y == -1)
        {
            Debug.Log("Å´");
        }
        else if (InputVector.x == -1 && InputVector.y == -1) 
        {
            Debug.Log("Å©Å´");
        }
        else if (InputVector.x == -1 && InputVector.y == 0)
        {
            Debug.Log("Å©");
        }
        else if (InputVector.x == -1 && InputVector.y == 1)
        {
            Debug.Log("Å©Å™");
        }
        else if (InputVector.x == 0 && InputVector.y == 1)
        {
            Debug.Log("Å™");
        }
        else if (InputVector.x == 1 && InputVector.y == 1)
        {
            Debug.Log("Å®Å™");
        }
        else if (InputVector.x == 1 && InputVector.y == 0)
        {
            Debug.Log("Å®");
        }
        else if (InputVector.x == 1 && InputVector.y == -1)
        {
            Debug.Log("Å®Å´");
        }
    }
}
