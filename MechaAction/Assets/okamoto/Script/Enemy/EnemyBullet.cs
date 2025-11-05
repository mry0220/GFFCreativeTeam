using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;
    private BurstEnemy _enemy;

    [SerializeField] private float _speed;
    private int _dir;

    [SerializeField] private int _damage;
    [SerializeField] private int _knockback;
    private string _name = "DamageEffect";

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _enemy = GameObject.FindWithTag("Enemy").GetComponent<BurstEnemy>();
    }

    private void Start()
    {
        _dir = _enemy._direction;
        StartCoroutine(_Destroy());
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;
        velocity.x = _dir * _speed;
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
        //Debug.Log("当たった");

        if (other.CompareTag("Player"))
        {
            var Interface = other.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir,_name);//敵のインターフェース<IDamage>取得
            }
        }
    }
}
