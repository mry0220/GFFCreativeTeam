using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float _speed;
    private int _dir;

    private int _damage;
    private int _knockback;
    private string _name = "DamageEffect";

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage ,int knockback ,int dir,string name)
    {
        _damage = damage;
        _knockback = knockback;
        _name = name;
        _dir = dir;
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;
        velocity.x = _dir * _speed;
        _rb.velocity = velocity;
        //Debug.Log(_dir);
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
