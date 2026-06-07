using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Player,
    Enemy,
    Neutral
}

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    public enum EntityState
    {
        Idle,
        Move,
        Attack,
        Down,
        Dead,
    }

    //rule is state can reference other script
    protected EntityState m_state;
    public EntityState State { get => m_state; }

    [SerializeField] protected TeamType m_team;
    public TeamType Team { get => m_team; }

    [SerializeField] private EntityDataSO m_entityData;

    //component-----------------
    protected Rigidbody m_rb;
    protected Animator m_anim;
    protected EntityHP m_classHP;
    //--------------------------

    //StateValue----------------
    protected Dictionary<StatusType, EntityStatus> m_status = new();

    public float MaxHP     { get => m_status[StatusType.HP].Value; }
    public float Speed     { get => m_status[StatusType.Speed].Value; }
    public float DashSpeed { get => m_status[StatusType.DashSpeed].Value; }
    public float Jump      { get => m_status[StatusType.Jump].Value; }
    public float Shield    { get => m_status[StatusType.Shield].Value; }
    //--------------------------

    //rule is flag can not reference other script
    //EntityFlag----------------
    protected bool m_IsDashing;

    protected bool m_IsGrounded;

    protected bool m_IsJumped;
    //--------------------------

    [Header("Check IsGrounded value")]
    [SerializeField] private Transform m_groundCheckOffset;
    [SerializeField] private float m_radius;
    [SerializeField] private float m_distance;
    [SerializeField] private LayerMask m_groundLayer;

    //variable------------------
    private int m_frontDir; // 1 or -1
    public Vector3 Forward
    {
        get
        {
            return new Vector3(m_frontDir, 0f, 0f);
        }
    }

    protected Vector2 m_moveDir; //entity dir (in value frome Entity.cs child script)

    protected Vector3 m_velocity; //save rigidbody value

    protected float m_fallTime; //use OnGravity() variable

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        m_classHP = GetComponent<EntityHP>();
    }

    protected virtual void Start()
    {
        m_status.Add(StatusType.HP,        new EntityStatus(m_entityData.MaxHP));
        m_status.Add(StatusType.Speed,     new EntityStatus(m_entityData.Speed));
        m_status.Add(StatusType.DashSpeed, new EntityStatus(m_entityData.DashSpeed));
        m_status.Add(StatusType.Jump,      new EntityStatus(m_entityData.Jump));
        m_status.Add(StatusType.Shield,    new EntityStatus(m_entityData.Shield));

        m_classHP.OnInitialized();

    }

    protected virtual void Update()
    {
        m_IsGrounded = IsGrounded();

        if(m_frontDir != 0)
        {
            float Yrot = m_frontDir > 0 ? 90f : 270f;
            transform.rotation = Quaternion.Euler(0, Yrot, 0);
        }
    }

    private bool IsGrounded()
    {
        return Physics.SphereCast(
            m_groundCheckOffset.position,
            m_radius,
            Vector3.down,
            out RaycastHit hit,
            m_distance,
            m_groundLayer
            );
    }

    #region View Gizmos
    private void OnDrawGizmos()
    {
        if(m_groundCheckOffset == null) return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(m_groundCheckOffset.position, m_radius);

        Vector3 end = m_groundCheckOffset.position + Vector3.down * m_distance;
        Gizmos.DrawWireSphere(end, m_radius);

        Gizmos.DrawLine(m_groundCheckOffset.position, end);
    }
    #endregion

    protected virtual void FixedUpdate()
    {
        //rule is we do not have to change m_rb.velocity.y because y is use Addforce
        //if  direct move to y is nothing addForce
        m_velocity = m_rb.linearVelocity;

        OnMove(m_moveDir);
        OnGravity();

        m_rb.linearVelocity = m_velocity;

    }

    private void OnMove(Vector2 dir)
    {
        if(m_IsDashing)
        {
            m_velocity.x = dir.x * DashSpeed;
        }
        else
        {
            m_velocity.x = dir.x * Speed;
        }

        if(dir.x == 0)
        {
            m_frontDir = 0;
        }
        else if(dir.x > 0)
        {
            m_frontDir = 1;
        }
        else
        {
            m_frontDir = -1;
        }
    }

    private void OnGravity()
    {
        if (m_IsGrounded) return;

        m_fallTime += Time.deltaTime;

        float fallSpeed = Physics.gravity.y * m_fallTime * 2f * 2f;

        m_velocity.y += fallSpeed * Time.fixedDeltaTime;

        m_velocity.y = Mathf.Max(m_velocity.y, -20f); //limit velocity.y speed
    }

    protected virtual void OnJump()
    {
        if (!m_IsGrounded) return;

        m_fallTime = 0f;
        m_rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);


    }

    public void OnTakeDamage(DamageData data, DamageResult result)
    {
        //hp
    }

    public EntityStatus GetStatus(StatusType type)
    {
        return m_status[type];
    }
}
