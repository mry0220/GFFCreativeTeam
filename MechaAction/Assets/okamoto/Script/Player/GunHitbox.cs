using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    //private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] DamageEffectSO _damageEffectSO;

    [SerializeField] GameObject _bulletPrefab;
    public Transform _bulletPosition;
    [SerializeField] private Transform _muzzlepoint;
    public GameObject _guneffect;

    private int _damage;
    private int _knockback;
    private string _name;
    private int _dir = 0;

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        //hitTargets.Clear();
    }

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[3].Damage;
        _knockback = _playerAttackSO.playerAttackList[3].Knockback;
        _name = _playerAttackSO.playerAttackList[3].EffectName;
        _dir = dir;

        var G_effect = Instantiate(_guneffect, _muzzlepoint.position, Quaternion.identity);
        Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

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

                    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _name);//ラムダ形式AIで知った
                    if (attackData != null && attackData.HitEffect != null)
                    {
                        var effect = Instantiate(attackData.HitEffect, hit.transform.position, Quaternion.identity);
                        Destroy(effect, 0.2f);
                    }
                }
            }
        }
    }

    public void ShotGun(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[4].Damage;
        _knockback = _playerAttackSO.playerAttackList[4].Knockback;
        _name = _playerAttackSO.playerAttackList[4].EffectName;
        _dir = dir;

        var G_effect = Instantiate(_guneffect, _muzzlepoint.position, Quaternion.identity);
        Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

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

                    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _name);//ラムダ形式AIで知った
                    if (attackData != null && attackData.HitEffect != null)
                    {
                        var effect = Instantiate(attackData.HitEffect, hit.transform.position, Quaternion.identity);
                        Destroy(effect, 0.2f);
                    }
                }
            }
        }
    }

    public void Rifle(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[5].Damage;
        _knockback = _playerAttackSO.playerAttackList[5].Knockback;
        _dir = dir;

        GameObject bullet = Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Initialize(_damage, _knockback, _dir, "DamageEffect");
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
