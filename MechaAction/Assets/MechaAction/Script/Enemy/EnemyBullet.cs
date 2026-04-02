using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;

    private TeamType m_team;

    private DamageData m_data;

    private Vector3 m_attackDir;

    private float _speed = 20f;
    Vector3 velocity;

    [SerializeField] private HitCollider m_hitCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(DamageData data,TeamType team)
    {
        m_data = data;
        m_team = team;

        m_attackDir = data.attackDir;
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    private void Update()
    {
        m_hitCollider.AttackCollider(m_data, m_team);
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;
        velocity.x = m_attackDir.x * _speed;
        _rb.velocity = velocity;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield break;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.gameObject.CompareTag("Player") ||
    //        !other.gameObject.CompareTag("PlayerWeapon") ||
    //        !other.gameObject.CompareTag("Enemy")) Destroy(gameObject);

    //    var Interface = other.GetComponent<IPlayerDamage>();
    //    if (Interface != null)
    //    {
    //        Interface.TakeDamage(m_damage, m_knockback, m_attackDir, m_effectname, m_audioname);
    //    }
    //}
}
