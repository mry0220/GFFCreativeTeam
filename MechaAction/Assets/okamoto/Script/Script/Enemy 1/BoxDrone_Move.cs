using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoxDrone_Move : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    [SerializeField] EnemyAttackSO _enemyattackSO;

    private int _clear;
    private int _hitdamage;
    private int _hitknockback;
    private int _dir;
    private string _effectname;
    private string _audioname;


    private Transform _player;
    private Rigidbody _rb;
    private float chaseRange = 10f;          // 視認距離
    private float moveSpeed = 5f;           // 追跡速度
    private float velocitySmoothTime = 0.3f; // 加速・減速の滑らかさ
    private int dir;

    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;

        _clear = GManager.Instance.clear;
        //Debug.Log(_clear);
        var attackData = _enemyattackSO.GetEffect("BoxDrone_Move");
        if (attackData != null)
        {
            _hitdamage = (int)(attackData.Hitdamage * (_clear * 1.5));
            _hitknockback = (int)(attackData.Hitknockback * (_clear * 1.5));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//右
            dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//左
            dir = -1;
        }

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        if (!CanMove) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Vector3 targetVelocity = direction * moveSpeed;

        // 滑らかに速度を変化させる
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref currentVelocity, velocitySmoothTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var Interface = other.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
            }
        }
    }

    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Move;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1.0f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
}
