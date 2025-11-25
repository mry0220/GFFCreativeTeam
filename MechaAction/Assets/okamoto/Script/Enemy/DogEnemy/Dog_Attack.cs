using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Attack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private DogEnemy _enemy;
    private int _dir;

    public LayerMask ignoreLayer;

    private int _clear;

    private void Awake()
    {
        _enemy = GetComponent<DogEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("DogEnemy");
        if (attackData != null)
        {
            _damage = attackData.Damage;
            _knockback = attackData.Knockback;
            _hitdamage = attackData.Hitdamage;
            _hitknockback = attackData.Hitknockback;
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        _dir = _enemy.Dir;
    }

    public void GunAttack()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f,~ignoreLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var Interface = hit.collider.GetComponent<IPlayerDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir,_effectname, _audioname);

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
