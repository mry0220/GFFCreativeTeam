using Critical;
using System.Collections;
using UnityEngine;

public class Burst_Attack : MonoBehaviour
{
    private int m_damage;
    private float m_criticalRate;
    private float m_criticalChance;
    private int m_knockback;
    private DamageType m_type;
    private float m_duration;

    [SerializeField] private BurstEnemy m_enemy;
    private Vector3 m_attackDir;

    private CriticalDamage m_criticaldamage = new CriticalDamage();

    [SerializeField] private GameObject _bulletPrefab;
    //[SerializeField] private GameObject _boundbulletPrefab;
    public Transform _bulletPosition;

    private int m_enhance;

    private void Awake()
    {

    }

    private void Start()
    {
        m_enhance = GManager.Instance.clear;
    }

    private void Update()
    {

    }

    [SerializeField] private AttackDataSO m_GAttackData;
    private Coroutine m_shootCoroutine;

    public void GunAttack()
    {
        DataApply(m_GAttackData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_enemy.Forward
        };

        if (m_shootCoroutine != null) return;

        m_shootCoroutine = StartCoroutine(Shoot(data));
    }

    public IEnumerator Shoot(DamageData data)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().Initialize(data, m_enemy.Team);
            yield return new WaitForSeconds(0.2f);
        }

        //if (m_enhance >= 2)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        GameObject bullet = Instantiate(_boundbulletPrefab, _bulletPosition.position, Quaternion.identity);
        //        bullet.GetComponent<EnemyBoundBullet>().Initialize(data, m_enemy.Team);
        //        yield return new WaitForSeconds(0.2f);
        //    }
        //}
        //else
        //{
        //    
        //}
        m_shootCoroutine = null;

        yield break;
    }

    public void DataApply(AttackDataSO data)
    {
        m_damage = data.Damage + (m_enhance * 10);
        m_criticalRate = data.CriticalRate + (float)(m_enhance * 0.5);
        m_criticalChance = data.CriticalChance + (float)(m_enhance * 5);
        m_knockback = data.Knockback + (m_enhance);
        m_type = data.Type;
        m_duration = data.Duration + (float)(m_enhance * 0.5);
    }
}
