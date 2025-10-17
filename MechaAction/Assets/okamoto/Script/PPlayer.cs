using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlayer : MonoBehaviour
{

    private Rigidbody _rb;

    private Vector2 _moveVector;

    private Vector2 _inputVector;

    private bool _isGrounded;
    private bool _isJump = false;
    private bool _isSecondJump;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    private float _fallTime;
    private float _fallSpeed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(_isJump);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isJump==true && _isSecondJump==false) return;
            _isJump = true;


            //if (_isJump)
            //{
            //    _isSecondJump = true;
            //}
    
        }
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        Vector3 velocity = _rb.velocity;　//一度変数にコピーしてから編集
        velocity.x = _moveVector.x * _moveSpeed;
        _moveVector.x = _inputVector.x; //ここに書くことで空中で左右に移動可能

        if (_isGrounded)
        {

            if( _isJump)
            {
                _rb.AddForce(0f, _jumpPower, 0f,ForceMode.Impulse);
                _isJump = false;
                Debug.Log("a");
            }
            _isSecondJump = true;
        }
        else
        {
            if (_isSecondJump && _isJump)
            {
                velocity.y = 0f;
                _fallTime = 0f;
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isSecondJump = false;
            }

            if(_isJump)
            {
                _isJump = false;
            }

            _fallTime += Time.deltaTime;

            //_moveVector.y = Physics.gravity.y * _FallTime * 2.0f;　//直接値を変えてしますので次のフレームで０に戻ってしまう
            _fallSpeed = Physics.gravity.y * _fallTime * 2f*2f; //Unityの標準重力に任せたいなら fallSpeed は不要


            //  --- 後々落下速度の制限 ---

            //Debug.Log(_fallSpeed);

            velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y速度に徐々に加算
                                                           //Time.fixedDeltaTime 物理演算をフレームレートに依存させないため必須
        }


        //_rb.velocity = _moveSpeed * _moveVector;

        _rb.velocity = velocity; //編集した値を戻してrigidbodyで実行
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _fallTime = 0f;
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded")) 
        {
            _fallTime = 0f;
            _isGrounded = false;
        }
    }
}
