using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Slash : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] DamageEffectSO _damageEffectSO;
    [SerializeField] private float _speed;

    private int _damage;
    private int _knockback;
    private int _dir;
    private string _effectname;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Initialize(int damage, int knockback, int dir, string name)
    {
        _damage = damage;
        _knockback = knockback;
        _effectname = name;
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
        //Debug.Log("当たった");

        if (other.CompareTag("Enemy"))
        {
            var Interface = other.GetComponent<IDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir);//敵のインターフェース<IDamage>取得

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 0.2f);
                }
            }
        }
    }

}
