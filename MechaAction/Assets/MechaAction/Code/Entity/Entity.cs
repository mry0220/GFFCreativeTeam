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
    protected float m_speed;
    protected float m_dashSpeed;
    protected float m_jump;
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
    protected Vector2 m_moveDir; //entity dir (in value frome Entity.cs child script)

    protected Vector3 m_velocity; //save rigidbody value

    protected float m_fallTime; //use OnGravity() variable

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponentInChildren<Animator>();
        //m_classHP = GetComponent<EntityHP>();
    }

    protected virtual void Start()
    {
        //m_classHP.OnInitialized(m_entityData);

        m_speed = m_entityData.Speed;
        m_jump = m_entityData.Jump;
    }

    protected virtual void Update()
    {
        m_IsGrounded = IsGrounded();
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
            m_velocity.x = dir.x * m_dashSpeed;
        }
        else
        {
            m_velocity.x = dir.x * m_speed;
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
        m_rb.AddForce(Vector3.up * m_jump, ForceMode.Impulse);


    }

    public void OnTakeDamage()
    {
        //hp
    }
}
