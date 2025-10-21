using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public enum EnemyState { Idle, ArcingJump, Dropping, Cooldown}

    [SerializeField] private Transform player;
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private float _dropForce = 30f;
    [SerializeField] private float _detectionRange = 7f;
    [SerializeField] private float _cooldownTime = 2f;

    private Rigidbody rb;
    private EnemyState currentState = EnemyState.Idle;
    private float cooldownTimer = 0f;

    private void Awake()
    {
         rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                float xDistance = Mathf.Abs(transform.position.x - player.position.x);
                float totalDistance = Vector3.Distance(transform.position, player.position);

                if(xDistance <= 3.5f)
                {
                    Debug.Log("ãﬂê⁄çUåÇ");
                }
                else if(totalDistance < _detectionRange)
                {
                    ArcJump();
                }
                    break;

            case EnemyState.ArcingJump:
                Vector3 horizontalOffset = transform.position - player.position;
                horizontalOffset.y = 0;

                if (horizontalOffset.magnitude < 1.0f && transform.position.y >= player.position.y + 1.0f)
                {
                    Drop();
                }

                else if(transform.position.y >= 10f)
                {
                    Drop();
                }
                    break;

            case EnemyState.Dropping:
                // óéâ∫íÜÇÕï®óùââéZÇ…îCÇπÇÈ
                break;

            case EnemyState.Cooldown:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    currentState = EnemyState.Idle;
                }
                break;
        }
    }

    private void ArcJump()
    {
        Vector3 Direction = (player.position - transform.position).normalized;
        Vector3 arcVelocity = new Vector3(Direction.x,_jumpPower, Direction.z);
        rb.velocity = arcVelocity;
        currentState = EnemyState.ArcingJump;
    }

    private void Drop()
    {
        rb.velocity = new Vector3(0, -_dropForce, 0);
        currentState = EnemyState.Dropping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == EnemyState.Dropping)
        {
            currentState = EnemyState.Cooldown;
            cooldownTimer = _cooldownTime;
        }
    }

}