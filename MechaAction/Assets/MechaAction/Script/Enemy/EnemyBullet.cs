using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody _rb;

    private int _damage;
    private int _knockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    private float _speed = 20f;
    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage ,int knockback ,int dir ,string effectname ,string audioname)
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
        if (!other.gameObject.CompareTag("Player") ||
            !other.gameObject.CompareTag("PlayerWeapon") ||
            !other.gameObject.CompareTag("Enemy")) Destroy(gameObject);

        var Interface = other.GetComponent<IPlayerDamage>();
        if (Interface != null)
        {
            Interface.TakeDamage(_damage, _knockback, _dir, _effectname, _audioname);
        }
    }
}
