using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundBullet : MonoBehaviour
{
    private Rigidbody _rb;

    private int _damage;
    private int _knockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    private float _speed = 13f;
    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage, int knockback, int dir, string effectname, string audioname)
    {
        _damage = damage;
        _knockback = knockback;
        _effectname = effectname;
        _audioname = audioname;
        _dir = dir;
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    private void FixedUpdate()
    {
        velocity = _rb.linearVelocity;
        velocity.x = _dir * _speed;
        _rb.linearVelocity = velocity;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (!other.gameObject.CompareTag("Player") ||
        //    !other.gameObject.CompareTag("PlayerWeapon") ||
        //    !other.gameObject.CompareTag("Enemy")) Destroy(gameObject);

        var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
        if (Interface != null)
        {
            Interface.TakeDamage(_damage, _knockback, _dir, _effectname, _audioname);
        }
    }
}
