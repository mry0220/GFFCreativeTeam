using System.Collections;
using UnityEngine;

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

public class Player : MonoBehaviour, ITeam
{
    private PlayerState m_state;
    public PlayerState State { get => m_state; }

    [SerializeField] private TeamType m_team;
    public TeamType Team { get => m_team; }

    public bool IsDead => m_state == PlayerState.Dead;

    public bool CanMove => m_state == PlayerState.Standing;

    private Rigidbody m_rb;
    private Animator m_anim;
    private DirectionTarget m_dirtarget;

    private Vector2 _moveVector;
    private Vector2 _inputVector;

    private Vector3 velocity;

    [SerializeField] private PlayerDataSO m_playerData;
    private float m_moveSpeed;
    private float m_jumpPower;


    private bool m_isGrounded;
    private bool _isJump = false;
    private bool _isSecondJump;
    private bool _isRun = false;
    private int _Runcount = 0;

    private Vector3 m_forward;
    public Vector3 Forward { get => m_forward; }

    private float prevHorizontal = 0f;
    //private bool _isDash = false;//ダッシュ中向きが変わらないように
    private bool _canDash = true;//空中で２回目ダッシュを防ぐため
    public bool _isBan= false;

    private float _fallTime;

    private float _SPEED = 0f;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        m_dirtarget = GetComponent<DirectionTarget>();

        m_moveSpeed = m_playerData.Speed;
        m_jumpPower = m_playerData.JumpPower;
    }

    private void Start()
    {
        m_state = PlayerState.Standing;

        ApplySkillUpgrades();
    }
    private void ApplySkillUpgrades()
    {
        //if (SkillManager.Instance.HasSkill(SkillType.SHOTGUN))
        //{
        //    _SPEED += 0.1f;
        //    Debug.Log("スピードアップ！");
        //}
        //if (SkillManager.Instance.HasSkill(SkillType.RIFLE))
        //{
        //    _SPEED += 0.2f;
        //    Debug.Log("スピードアップ！");
        //}
        //if (SkillManager.Instance.HasSkill(SkillType.KNOCKP1))
        //{
        //    _SPEED += 0.2f;
        //    Debug.Log("スピードアップ！");
        //}
    }

    private void Update()
    {
        _MousePosition();//マウスの位置取得

        if(m_forward.x != 0)
        {
            float Yrot = m_forward.x > 0 ? 90f : 270f;
            transform.rotation = Quaternion.Euler(0, Yrot, 0);
        }
        
        _InputDetection();//ダッシュの２回入力検知

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJump = true;

        }

        if (Input.GetMouseButtonDown(1) && _canDash)
        {
            m_anim.SetFloat("Speed", 3);
            //StartCoroutine(_Dash());
            _canDash = false;
        }

        m_isGrounded = IsGrounded();
    }

    [SerializeField] private Transform m_groundCheck;
    [SerializeField] private float m_radius = 0.3f;
    [SerializeField] private float m_checkDistance = 0.5f;
    [SerializeField] private LayerMask m_groundLayer;

    bool IsGrounded()
    {
        return Physics.SphereCast(
            m_groundCheck.position,
            m_radius,
            Vector3.down,
            out RaycastHit hit,
            m_checkDistance,
            m_groundLayer
        );
    }

    private void OnDrawGizmos()
    {
        if (m_groundCheck == null) return;

        Gizmos.color = Color.yellow;

        // 開始地点
        Gizmos.DrawWireSphere(m_groundCheck.position, m_radius);

        // 終点
        Vector3 end = m_groundCheck.position + Vector3.down * m_checkDistance;
        Gizmos.DrawWireSphere(end, m_radius);

        // 間を線でつなぐ
        Gizmos.DrawLine(m_groundCheck.position, end);
    }

    private void FixedUpdate()
    {
        _inputVector.x = Input.GetAxisRaw("Horizontal");
        _inputVector.y = Input.GetAxisRaw("Vertical");

        if (m_state == PlayerState.Dash) return;

        velocity = m_rb.velocity; //一度変数にコピーしてから編集
        _moveVector.x = _inputVector.x; //ここに書くことで空中で左右に移動可能

        Move();

        _Jump();

        #region　歩きアニメーション
        if(m_isGrounded)
        {
            //Debug.Log("moveanimation");
            if (velocity.x >= -1 && velocity.x <= 1)
            {
                m_anim.SetFloat("Speed", 0);
                //Debug.Log("Speed0");
            }
            else if ((velocity.x > 1 && velocity.x <= 4) || (velocity.x < 1 && velocity.x >= -4))
            {
                m_anim.SetFloat("Speed", 1);
                //Debug.Log("Speed1");
            }
            else
            {
                m_anim.SetFloat("Speed", 2);
                //Debug.Log("Speed2");
            }
        }
        else
        {
            //if(velocity.y > 0)
            //{
            //    _anim.SetInteger("JumpSpeed", 0);
            //}
            //else
            //{
            //    _anim.SetInteger("JumpSpeed", 1);
            //}
        }

        #endregion

        if (!m_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0f;
        }

        m_rb.velocity = velocity; //編集した値を戻してrigidbodyで実行
    }

    public void ChangeState(PlayerState newState)
    {
        m_state = newState;
        if(newState == PlayerState.Attack && m_isGrounded)
        {
            m_rb.velocity = Vector3.zero;
        }
    }

    public void _ReturnNormal()
    {
        m_state = PlayerState.Standing;
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

        Vector3 currentPos;

        if(m_dirtarget.CurrentTarget != null)
        {
            currentPos = m_dirtarget.CurrentTarget.position;
            //Debug.Log("target");
        }
        else
        {
            currentPos = worldPos;
        }

        if (m_state == PlayerState.Dash) return;

        Vector3 dir = currentPos - transform.position;

        if(Mathf.Abs(dir.x) > 0.01f)
        {
            m_forward = new Vector3(Mathf.Sign(dir.x), 0, 0);
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

    private void Move()
    {
        if(!CanMove) return;

        if (_isRun)
        {
            velocity.x = _moveVector.x * (m_moveSpeed + _SPEED);
        }
        else
        {
            velocity.x = _moveVector.x * (m_moveSpeed + _SPEED) * 0.5f;
        }

        //if (_lookDir == 1)
        //{
        //    if (_isRun && _moveVector.x > 0f)
        //    {
        //        velocity.x = _moveVector.x * (_moveSpeed + _SPEED);
        //    }
        //    else
        //    {
        //        velocity.x = _moveVector.x * (_moveSpeed + _SPEED) * 0.5f;
        //    }
        //}
        //else if (_lookDir == -1)
        //{
        //    if (_isRun && _moveVector.x < 0f)
        //    {
        //        velocity.x = _moveVector.x * (_moveSpeed + _SPEED);
        //    }
        //    else
        //    {
        //        velocity.x = _moveVector.x * (_moveSpeed + _SPEED) * 0.5f;
        //    }
        //}

    }

    private void _Jump()
    {
        if (!CanMove) return;
        if (_isBan) return;//PlayerHPで管理したbool

        if (m_isGrounded)
        {
            if (_isJump)
            {
                m_anim.SetTrigger("Jump");
                _fallTime = 0f;
                m_rb.AddForce(0f, m_jumpPower, 0f, ForceMode.Impulse);
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
                m_anim.SetTrigger("Jump");
                m_rb.AddForce(0f, m_jumpPower, 0f, ForceMode.Impulse);
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

    //private IEnumerator _Dash()
    //{
    //    if(!CanMove) yield break;

    //    ChangeState(PlayerState.Dash);
    //    Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
    //        LayerMask.NameToLayer("Enemy"), true);
    //    //_isDash = true;
    //    Vector3 velocity = m_rb.velocity;
    //    _fallTime = 0f;

    //    float t = 0f;
    //    float duration = 0.3f;
    //    while (t < duration)
    //    {
    //        velocity = m_rb.velocity;

    //        velocity.x = m_forward.x * 30f;
    //        velocity.y = 0f;

    //        //Debug.Log(velocity.x);
    //        m_rb.velocity = velocity;
    //        t += Time.deltaTime;
    //        yield return new WaitForFixedUpdate();  //コルーチン内でFixedUpdateできるのAIで知った
    //    }
    //    Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
    //        LayerMask.NameToLayer("Enemy"), false);
    //    _ReturnNormal();
    //    //_isDash = false;
    //    yield break;
    //}

    public void SKnockBack(int dir,int knockback)
    {
        m_rb.velocity = Vector3.zero;
        m_rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        m_anim.SetTrigger("SKnock");
    }

    public void BKnockBack(int dir, int knockback)
    {
        m_rb.velocity = Vector3.zero;
        m_rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        m_anim.SetTrigger("BKnock");
    }

    public void Stun()//電撃ダメージで呼ぶ
    {
        m_rb.velocity = Vector3.zero;
    }


    public void Dead()
    {
        m_rb.velocity = Vector3.zero;
        m_anim.SetInteger("Dead", 1);
    }

    public void Respawn()
    {
        m_anim.SetInteger("Dead", 0);

    }

    private IEnumerator Gimmick()
    {
        yield return new WaitForSeconds(0.8f);
        _ReturnNormal();

        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("JumpGimmick"))
        //{
        //    _fallTime = 0f;
        //    m_rb.velocity = Vector3.zero;
        //    ChangeState(PlayerState.Other);
        //    StartCoroutine(Gimmick());
        //    m_rb.AddForce(0f,23f,0f, ForceMode.Impulse);
        //}

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
