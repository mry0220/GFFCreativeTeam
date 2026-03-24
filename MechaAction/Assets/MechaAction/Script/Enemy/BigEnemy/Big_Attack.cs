using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private BigEnemy _enemy;
    private int _dir;

    private int _clear;

    private void Awake()
    {
        _enemy = GetComponent<BigEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("BigEnemy");
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

    public void Attack()
    {
        Debug.Log("Attack");
    }

    public void JumpAttack()
    {
        Debug.Log("JumpAttack");

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
