using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;

    private TeamType m_team;

    private DamageData m_data;

    private Vector3 m_attackDir;

    [SerializeField] private EffectDataSO m_overrideEffectData;
    [SerializeField] private AudioDataSO m_overrideAudioData;

    private float _speed = 20f;
    Vector3 velocity;

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
        //m_hitCollider.AttackCollider(m_data, m_team);
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

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponentInParent<IDamage>();
        if (damageable == null) return;

        var team = other.GetComponentInParent<ITeam>();
        if (team != null)
        {
            // ìØÇ∂É`Å[ÉÄÇ»ÇÁñ≥éã
            if (team.Team == m_team) return;
        }
        else
        {
            return;
        }

        DamageResult result = new DamageResult
        {
            hitPoint = transform.position,
            hitNormal = transform.position - 
            (other.transform.position).normalized,

            overrideEffectData = m_overrideEffectData,
            overrideAudioData = m_overrideAudioData
        };

        damageable.TakeDamage(m_data, result);

        Destroy(gameObject);
    }
}
