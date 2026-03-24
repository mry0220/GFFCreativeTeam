using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAndFire_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private ShieldAndFire _enemy;
    private int _dir;

    //public Transform _bulletPosition;

    public LayerMask ignoreLayer;

    private int _clear;

    private void Awake()
    {
        _enemy = GetComponent<ShieldAndFire>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("ShieldAndFire");
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

    public void Fire()
    {
        if (Physics.BoxCast(transform.position, Vector3.one * 2f, transform.forward * 0.5f, out RaycastHit hit,
            Quaternion.identity, 7f, ~ignoreLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var Interface = hit.collider.GetComponent<IPlayerDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
                }
            }
        }
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
