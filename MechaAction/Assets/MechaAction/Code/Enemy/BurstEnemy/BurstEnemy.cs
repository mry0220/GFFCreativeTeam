using System.Collections;
using UnityEngine;

public class BurstEnemy : MonoBehaviour, IEnemy, ITeam
{
    private enum EnemyState { 
        Look,          //探す
        Move,          //追跡
        Wait,          //発射用意(いらない)
        Attack        //発射
    }

    private EnemyState _state = EnemyState.Look;

    private bool m_canMove;
    private float m_canMoveTime;

    private bool m_isTurn = false;
    private float m_tuenTime;

    [SerializeField] private TeamType m_team;
    public TeamType Team { get => m_team; }

    private Transform _player;
    private Rigidbody _rb;
    private Animator _anim;
    private Burst_Attack m_attack;

    private float _moveSpeed = 1.0f;

    private Vector3 m_forward;
    public Vector3 Forward { get => m_forward; }

    private int m_moveDir = -1;//初期左向き
    private int m_nextDir;
 
    private float _attacktime;

    Vector3 velocity;

    private float _fallTime;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        m_attack = GetComponent<Burst_Attack>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        #region 時間経過
        if (m_isTurn)
        {
            m_tuenTime -= Time.deltaTime;

            if(m_tuenTime < 0)
            {
                m_moveDir = m_nextDir;
                m_isTurn = false;
            }
        }

        if(m_canMove)
        {
            m_canMoveTime -= Time.deltaTime;

            if( m_canMoveTime < 0)
            {
                m_canMove = true;
            }
        }
        #endregion

        m_forward = new Vector3(m_moveDir, 0, 0);

        float Yrot = m_forward.x > 0 ? 90f : 270f;
        transform.rotation = Quaternion.Euler(0, Yrot, 0);

        _isGrounded = IsGrounded();
    }
    [Header("Ground Check")]
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
        velocity = _rb.velocity;

        if (!_isGrounded)
        {
            Gravity();
        }
        else
        {
            _fallTime = 0f;
        }

        switch (_state)
        {
            case EnemyState.Look:
                Look();
                _attacktime = 0.0f;
                if (Vector3.Distance(transform.position, _player.position) < 10f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                _attacktime += Time.deltaTime;
                Move();
                
                if (Vector3.Distance(transform.position, _player.position) > 20f)
                {
                    _state = EnemyState.Look;
                }
                else if (_attacktime > 2f)
                {
                    _state = EnemyState.Attack;
                }
                break;

            case EnemyState.Attack:
                Attack();
                _attacktime = 0.0f;
                _state = EnemyState.Move;
                break;
        }

        
        _rb.velocity = velocity;
    }

    private void Look()
    {
        _anim.SetInteger("Speed", 0);
    }

    private void Move()
    {
        int targetDir = _player.position.x > _rb.position.x ? 1 : -1;

        if(!m_isTurn && targetDir != m_moveDir)
        {
            Tuen(targetDir);
            return;
        }


        if(m_isTurn)
        {
            velocity.x = 0f;
            _anim.SetInteger("Speed", 0);
        }
        else
        {
            velocity.x = m_moveDir * _moveSpeed;
            _anim.SetInteger("Speed", 1);
        }
    }

    private void Tuen(int newDir)
    {
        m_isTurn = true;
        m_tuenTime = 2.0f;
        m_nextDir = newDir;
    }

    private void Gravity()
    {
        _fallTime += Time.deltaTime;

        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unityの標準重力に任せたいなら fallSpeed は不要

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y速度に徐々に加算
                                                        //Time.fixedDeltaTime 物理演算をフレームレートに依存させないため必須
        if (velocity.y < -20f)//落下速度の制限
        {
            velocity.y = -20f;
        }
    }

    private void Attack()
    {
        m_attack.GunAttack();
    }

    public void Stun(float time = 0.5f)
    {
        m_canMoveTime = time;
        m_canMove = false;
    }


    #region 被ダメ処理
    public void KnockBack(Vector3 attackDir,int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(attackDir.x * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);

        Stun();
        //anim
    }

    public void ElectStun(float duration)
    {
        Stun(duration);
    }
    #endregion

}
