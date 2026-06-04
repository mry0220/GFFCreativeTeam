using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBall : MonoBehaviour
{
    private Rigidbody _rb;

    private int _damage;
    private int _knockback;
    private float _electtime;
    private string _effectname;
    private string _audioname;

    private int _dir;

    private Vector3 velocity;
    private float _movespeed = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(_Destroy());
    }

    public void Initialize(int damage,int knockback,int dir,float electtime,string effectname,string audioname)
    {
        _damage = damage;
        _knockback = knockback;
        _electtime = electtime;
        _effectname = effectname;
        _audioname = audioname;
        _dir = dir;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;

        velocity.x = _movespeed * _dir;

        _rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Interface = other.GetComponent<IPlayerDamage>();
        if (Interface != null)
        {
            Interface.TakeElectDamage(_damage,_knockback, _dir,_electtime, _effectname, _audioname);//敵のインターフェース<IDamage>取得
        }
    }
}
