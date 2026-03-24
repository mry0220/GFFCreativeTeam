using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChild_Attack : MonoBehaviour
{
    [SerializeField] DamageEffectSO _damageEffectSO;
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private SpiderChild _enemy;
    private int _dir;

    private int _clear;

    private void Awake()
    {
        _enemy = GetComponent<SpiderChild>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("SpiderChild");
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

    public void Boom()
    {
        Debug.Log("Boom");
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // IDamageable 実装クラスへダメージ
                var Interface = hit.GetComponent<IPlayerDamage>();
                if (Interface != null) Interface.TakeDamage(_damage, _knockback, _dir,_effectname, _audioname);

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == "DamageEffect");//ラムダ形式AIで知った
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 0.2f);
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