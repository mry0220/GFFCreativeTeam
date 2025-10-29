using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SDrone_chase : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float chaseRange = 5f;          // 視認距離
    [SerializeField] private float moveSpeed = 5f;           // 追跡速度
    [SerializeField] private float stopDuration = 0.5f;
    [SerializeField] private float velocitySmoothTime = 0.3f; // 加速・減速の滑らかさ

    private Rigidbody rb;
    private bool isStopped = false;
    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (isStopped)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetVelocity = direction * moveSpeed;

        // 滑らかに速度を変化させる
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, velocitySmoothTime);
    }

   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            StartCoroutine(StopTemporarily());
        }
    }

    private IEnumerator StopTemporarily()
    {
        isStopped = true;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(stopDuration);
        isStopped = false;
    }*/
}