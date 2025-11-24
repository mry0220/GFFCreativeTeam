using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemyAttack : MonoBehaviour
{
    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _damage;
    private int _knockback;
    private int _hitdamage;
    private int _hitknockback;
    private float _electtime;
    private string _effectname;
    private string _audioname;

    private ElectricEnemy _enemy;
    private int _dir;

    [SerializeField] private GameObject _groundattack;

    private int _clear;

    public LayerMask ignoreLayer;

    private void Awake()
    {
        _enemy = GetComponent<ElectricEnemy>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("ElectricEnemy");
        if (attackData != null)
        {
            _damage = (int)(attackData.Damage + (_clear * 10));
            _knockback = (int)(attackData.Knockback + (_clear * 2));
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _electtime = attackData.Electtime + (_clear * 1);
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

        _dir = _enemy.Dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
            }
        }
    }

    public void ElectricGroundAttack()
    {
        GameObject _attack = Instantiate(_groundattack, transform.position, Quaternion.identity);
        _attack.GetComponent<GroundBall>().Initialize(_damage, _knockback, _dir,_electtime, _effectname, _audioname);
    }

    public void ElectricAttack()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, ~ignoreLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                var Interface = hit.collider.GetComponent<IPlayerDamage>();
                if (Interface != null)
                {
                    Interface.TakeElectDamage(_damage,_knockback, _dir,_electtime,_effectname, _audioname);//敵のインターフェース<IDamage>取得
                }
            }
        }
    }
}
