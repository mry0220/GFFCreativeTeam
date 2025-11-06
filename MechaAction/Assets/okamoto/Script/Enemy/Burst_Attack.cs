using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Attack : MonoBehaviour
{
    private BurstEnemy _enemy;

    [SerializeField] GameObject _bulletPrefab;
    public Transform _bulletPosition;

    [SerializeField] private int _damage = 3;
    [SerializeField] private int _knockback = 0;
    [SerializeField] private int _dir;
    [SerializeField] private string _name = "DamageEffect";

    private void Awake()
    {
        _enemy = GetComponent<BurstEnemy>();
    }

    private void Update()
    {
        _dir = _enemy._direction;
    }

    public void GunAttack()
    {
        StartCoroutine(Gun());
    }

    private IEnumerator Gun()
    {
        for(int i = 0;i < 3; i++)
        {
            Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir,_name);//敵のインターフェース<IDamage>取得
            }
        }
    }
}
