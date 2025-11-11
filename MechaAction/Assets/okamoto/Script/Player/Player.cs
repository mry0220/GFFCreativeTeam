using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.UI.Image;

public enum PlayerState
{
    Standing,    //立ち、動く、ジャンプ
    Dash,
    Attack,
    Knockback,
    Other,　　　//ギミック、電気、妨害
    Dead,       //死んだら状態遷移防止
    Respawn     //唯一Deadをすり抜ける
}

public class Player : MonoBehaviour
{

    private PlayerState _state = PlayerState.Standing;
    private PlayerState state
    {
        get => _state;
        set
        {
            Debug.Log("変えたい");
            if (IsDead　&& value != PlayerState.Respawn)//DeadかつRespawn以外
            {
                return;
            }
            _state = value;
        }
    }

    public bool IsDead => _state == PlayerState.Dead;

    public bool CanMove => _state == PlayerState.Standing;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private Animator _anim;

    private Vector2 _moveVector;
    private Vector2 _inputVector;

    private Vector3 velocity;

    private bool _isGrounded;
    private bool _isJump = false;
    private bool _isSecondJump;
    private bool _isRun = false;
    private int _Runcount = 0;
    public int _lookDir;
    private float prevHorizontal = 0f;
    private bool _isDash = false;//ダッシュ中向きが変わらないように
    private bool _canDash = true;//空中で２回目ダッシュを防ぐため
    public bool _isBan= false;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpPower;

    Vector3 origin;

    private float _fallTime;

    private bool _isRight = false;
    private bool _isLeft = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _MousePosition();//マウスの位置取得
        
        _InputDetection();//ダッシュの２回入力検知

        if (_isRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _isRight = false;
        }
        else if (_isLeft)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            _isLeft = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;

        }

        if (Input.GetMouseButtonDown(1) && _canDash)
        {
            _anim.SetFloat("Speed", 3);
            StartCoroutine(_Dash());
            _canDash = false;
        }

        Vector2 bounds = _col.bounds.size;
        RaycastHit hit;
        origin = transform.position + Vector3.down * (bounds.y / 2);
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));

        //Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        //Debug.Log(velocity.x);
        //Debug.Log(_isGrounded);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.4f);
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        if (_state == PlayerState.Dash) return;

        velocity = _rb.velocity; //一度変数にコピーしてから編集
        _moveVector.x = _inputVector.x; //ここに書くことで空中で左右に移動可能

        _Move();

        _Jump();

        #region　歩きアニメーション

        if(_isGrounded)
        {
            if (velocity.x >= -1 && velocity.x <= 1)
            {
                _anim.SetFloat("Speed", 0);
                //Debug.Log("Speed0");
            }
            else if ((velocity.x > 1 && velocity.x <= 4) || (velocity.x < 1 && velocity.x >= -4))
            {
                _anim.SetFloat("Speed", 1);
                //Debug.Log("Speed1");
            }
            else
            {
                _anim.SetFloat("Speed", 2);
                //Debug.Log("Speed2");
            }
        }
        else
        {
            if(velocity.y > 0)
            {
                _anim.SetInteger("JumpSpeed", 0);
            }
            else
            {
                _anim.SetInteger("JumpSpeed", 1);
            }
        }

        #endregion

        if (!_isGrounded)
        {
            _Gravity();
        }

        _rb.velocity = velocity; //編集した値を戻してrigidbodyで実行
    }

    public void _ChangeState(PlayerState newState)
    {
        Debug.Log("change");
        state = newState;
        //Debug.Log("stateが" + newState + "に変わった");
        if(newState == PlayerState.Attack && _isGrounded)
        {
            //Debug.Log("chengestate動きを止める");
            _rb.velocity = Vector3.zero;
        }
    }

    public void _ReturnNormal()
    {
        state = PlayerState.Standing;
    }

    private void _MousePosition()
    {
        // マウスのスクリーン座標を取得
        Vector3 mousePos = Input.mousePosition;

        //Debug.Log($"Screen Position: X={mousePos.x}, Y={mousePos.y}");

        // ワールド座標に変換（カメラ必須）
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePos.x, mousePos.y, 10f)//Camera.main.nearClipPlane
        );


        if (transform.position.x < worldPos.x && !_isDash)
        {
            _lookDir = 1;
            _isRight = true;
        }
        else if (transform.position.x > worldPos.x && !_isDash)
        {
            _lookDir = -1;
            _isLeft = true;
        }
    }

    private void _InputDetection()
    {
        if (prevHorizontal == 0f && (_inputVector.x == 1 || _inputVector.x == -1))//_inputVectorの押す瞬間を取得するため
        {
            if (_Runcount <= 0)
            {
                _Runcount = 150;
            }
            else
            {
                _isRun = true;
                //Debug.Log("Dash!");
            }
        }

        if (_Runcount > 0)
        {
            _Runcount--;
        }
        if (_inputVector.x == 0)
        {
            _isRun = false;
            //Debug.Log("nodash");
        }

        prevHorizontal = _inputVector.x;
    }

    private void _Move()
    {
        if(!CanMove) return;

        if (_lookDir == 1)
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
        
    }

    private void _Jump()
    {
        if (!CanMove) return;
        if (_isBan) return;//PlayerHPで管理したbool

        if (_isGrounded)
        {
            if (_isJump)
            {
                _anim.SetTrigger("Jump");
                _fallTime = 0f;
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isJump = false;
            }
            _isSecondJump = true;
            _canDash = true;
        }
        else
        {
            if (_isSecondJump && _isJump)
            {
                velocity.y = 0f;//二段目で跳ね上がり防ぎ
                _fallTime = 0f;
                _anim.SetTrigger("Jump");
                _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
                _isSecondJump = false;
            }
            if (_isJump)
            {
                _isJump = false;
            }
        }
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        //_moveVector.y = Physics.gravity.y * _FallTime * 2.0f;　//直接値を変えてしますので次のフレームで０に戻ってしまう
        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unityの標準重力に任せたいなら fallSpeed は不要

        //Debug.Log(_fallSpeed);

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y速度に徐々に加算
                                                        //Time.fixedDeltaTime 物理演算をフレームレートに依存させないため必須
        if (velocity.y < -20f)//落下速度の制限
        {
            velocity.y = -20f;
        }
    }

    private IEnumerator _Dash()
    {
        if(!CanMove) yield break;

        _ChangeState(PlayerState.Dash);

        _isDash = true;
        Vector3 velocity = _rb.velocity;
        _fallTime = 0f;

        float t = 0f;
        float duration = 0.2f;
        while (t < duration)
        {
            velocity = _rb.velocity;
            if (_lookDir == 1)
            {
                velocity.x = _lookDir * 15f;
                velocity.y = 0f;
            }
            else if (_lookDir == -1)
            {
                velocity.x = _lookDir * 15f;
                velocity.y = 0f;
            }
            //Debug.Log(velocity.x);
            _rb.velocity = velocity;
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();  //コルーチン内でFixedUpdateできるのAIで知った
        }
        _ReturnNormal();
        _isDash = false;
        yield break;
    }

    public void SKnockBack(int dir,int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        //anim
    }

    public void Stun()//電撃ダメージで呼ぶ
    {
        _rb.velocity = Vector3.zero;
    }


    public void Dead()
    {
        _rb.velocity = Vector3.zero;
        //anim
    }

    private IEnumerator Gimmick()
    {
        yield return new WaitForSeconds(0.8f);
        _ReturnNormal();

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpGimmick"))
        {
            _fallTime = 0f;
            _rb.velocity = Vector3.zero;
            _ChangeState(PlayerState.Other);
            StartCoroutine(Gimmick());
            _rb.AddForce(0f,23f,0f, ForceMode.Impulse);
        }

        //if (collision.gameObject.CompareTag("Grounded"))
        //{
        //    Debug.Log("ground");
        //    _fallTime = 0f;
        //    _isGrounded = true;
        //}
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Grounded"))
    //    {
    //        _fallTime = 0f;
    //        _isGrounded = false;
    //    }
    //}
}
