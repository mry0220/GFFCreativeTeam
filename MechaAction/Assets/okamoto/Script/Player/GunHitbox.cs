using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    //private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    //public int damage = 10;//プレイヤースクリプトで技によって変更
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] GameObject _bulletPrefab;
    public Transform _bulletPosition;
    private int _damage;
    private int _knockback;
    private int _dir = 0;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        //hitTargets.Clear();
    }

    private void Update()
    {

    }

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[3].Damage;
        _knockback = _playerAttackSO.playerAttackList[3].Knockback;
        _dir = dir;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var Interface = hit.collider.GetComponent<IDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir);//敵のインターフェース<IDamage>取得

                }
            }
        }
    }

    //問題　連続した攻撃の際、アニメーションのシグナルか何かでClear();を呼ぶ必要あり

    public void ShotGun(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[4].Damage;
        _knockback = _playerAttackSO.playerAttackList[4].Knockback;
        _dir = dir;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
        //Ray ray = new Ray();
        if (Physics.BoxCast(transform.position, Vector3.one * 0.5f, transform.forward, out RaycastHit hit,Quaternion.identity, 10f))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                var Interface = hit.collider.GetComponent<IDamage>();
                if (Interface != null)
                {
                    Interface.TakeDamage(_damage, _knockback, _dir);//敵のインターフェース<IDamage>取得

                }
            }
        }
    }

    public void Rifle(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[5].Damage;
        _knockback = _playerAttackSO.playerAttackList[5].Knockback;
        _dir += dir;

        Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("当たった");

    //    if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
    //    {
    //        //null条件

    //        other.GetComponent<IDamage>().TakeDamage(_damage, _knockback, _dir);//敵のインターフェース<IDamage>取得
    //        hitTargets.Add(other.gameObject);
    //    }
    //}
}
