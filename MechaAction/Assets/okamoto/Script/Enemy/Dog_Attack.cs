using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Attack : MonoBehaviour
{
    private DogEnemy _enemy;
    public LayerMask ignoreLayer;

    private int _damage = 20;
    private int _knockback = 5;
    private string _name = "DamageEffect";
    private int _dir;

    private void Awake()
    {
        _enemy = GetComponent<DogEnemy>();
    }

    private void Update()
    {
        //transform.position += transform.forward * 3f * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        _dir = _enemy._direction;
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
                    Interface.TakeDamage(_damage, _knockback, _dir,_name);//敵のインターフェース<IDamage>取得

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
                Interface.TakeDamage(_damage, _knockback, _dir, _name);//敵のインターフェース<IDamage>取得
            }
        }
    }
}
