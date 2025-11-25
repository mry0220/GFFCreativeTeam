using System.Collections;
using UnityEngine;

public class Burst_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private BurstEnemy _enemy;
    private int _dir;
    
    [SerializeField] private GameObject _bulletPrefab;
    public Transform _bulletPosition;

    private int _clear;

    private void Awake()
    {
        _enemy = GetComponent<BurstEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("BurstEnemy");
        if (attackData != null)
        {
            _damage = (int)(attackData.Damage + (_clear * 10));
            _knockback = (int)(attackData.Knockback + (_clear * 2));
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        _dir = _enemy.Dir;
    }

    public IEnumerator GunAttack()
    {
        for(int i = 0;i < 3; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().Initialize(_damage ,_knockback,_dir,_effectname , _audioname);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);
            }
        }
    }
}
