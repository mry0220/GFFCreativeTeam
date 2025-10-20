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
    private bool _isRun = false;
    private int _Runcount = 0;
    private int _lookDir;
    private float prevHorizontal = 0f;
    private bool _isDash = false;

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
        // マウスのスクリーン座標を取得
        Vector3 mousePos = Input.mousePosition;

        //Debug.Log($"Screen Position: X={mousePos.x}, Y={mousePos.y}");

        // ワールド座標に変換（カメラ必須）
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y,10f )//Camera.main.nearClipPlane
        );
        

        if(transform.position.x  < worldPos.x)
        {
            _lookDir = 1;
        }
        else if (transform.position.x > worldPos.x)
        {
            _lookDir = -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;
    
        }


        if (prevHorizontal == 0f && _inputVector.x == 1)//_inputVectorの押す瞬間を取得するため
        {
            if(_Runcount <= 0)
            {
                _Runcount = 100;
            }
            else
            {
                _isRun = true;
                Debug.Log("Dash!");
            }
        }

        if (prevHorizontal == 0f && _inputVector.x == -1)//_inputVectorの押す瞬間を取得するため
        {
            if (_Runcount <= 0)
            {
                _Runcount = 100;
            }
            else
            {
                _isRun = true;
                Debug.Log("Dash!");
            }
        }

        if (_Runcount > 0)
        {
            _Runcount--;
        }
        if(_inputVector.x == 0)
        {
            _isRun = false;
            Debug.Log("nodash");
        }

        prevHorizontal = _inputVector.x;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isDash = true;
        }
         
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        Vector3 velocity = _rb.velocity; //一度変数にコピーしてから編集
        _moveVector.x = _inputVector.x; //ここに書くことで空中で左右に移動可能

        if(_isDash)
        {
            StartCoroutine(_Dash());
            return;
        }

        if(_lookDir == 1)
        {
            if (_isRun && _moveVector.x > 0f)
            {
                velocity.x = _moveVector.x * _moveSpeed;

            }
            else
            {
                velocity.x = _moveVector.x * _moveSpeed * 0.5f;
            }
        }
        else if (_lookDir == -1)
        {
            if (_isRun && _moveVector.x < 0f)
            {
                velocity.x = _moveVector.x * _moveSpeed;

            }
            else
            {
                velocity.x = _moveVector.x * _moveSpeed * 0.5f;
            }
        }


        if (_isGrounded)
        {

            if( _isJump)
            {
                _rb.AddForce(0f, _jumpPower, 0f,ForceMode.Impulse);
                _isJump = false;
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

    private IEnumerator _Dash()
    {

        yield break;
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
