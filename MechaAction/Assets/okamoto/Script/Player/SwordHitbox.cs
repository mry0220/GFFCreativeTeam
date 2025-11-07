using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    //public int damage = 10;//プレイヤースクリプトで技によって変更
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] DamageEffectSO _damageEffectSO;

    [SerializeField] GameObject _slashPrefab;
    [SerializeField] private Transform _groundpoint;
    public GameObject _groundeffect;



    private Collider _collider;
    public Transform _slashPosition;
    private int _damage;
    private int _knockback;
    private string _effectname;
    private int _dir = 0;

    private bool _groundattack = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _collider.enabled = false; // スクリプトより先に物理無効化
    }

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        hitTargets.Clear();
        _collider.enabled = true;
        _groundattack = false;
    }

    private void Update()
    {
        
    }

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[0].Damage;
        _knockback = _playerAttackSO.playerAttackList[0].Knockback;
        _effectname = _playerAttackSO.playerAttackList[0].EffectName;
        _dir = dir;
    }

    //問題　連続した攻撃の際、アニメーションのシグナルか何かでClear();を呼ぶ必要あり

    public void tatakitukeAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[1].Damage;
        _knockback = _playerAttackSO.playerAttackList[1].Knockback;
        _effectname = _playerAttackSO.playerAttackList[1].EffectName;
        _dir = dir;
        _groundattack = true;

    }

    public void slashAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[2].Damage;
        _knockback = _playerAttackSO.playerAttackList[2].Knockback;
        _effectname = _playerAttackSO.playerAttackList[2].EffectName;
        _dir = dir;

        GameObject slash = Instantiate(_slashPrefab, _slashPosition.position, gameObject.transform.rotation);
        slash.GetComponent<Slash>().Initialize(_damage, _knockback, _dir, _effectname);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            var Interface = other.GetComponent<IDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_damage, _knockback, _dir);//敵のインターフェース<IDamage>取得
                hitTargets.Add(other.gameObject);

                var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                if (attackData != null && attackData.HitEffect != null)
                {
                    var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                    Destroy(effect, 0.2f);
                }
            }
        }

        if (other.CompareTag("Grounded") && _groundattack)
        {
            var G_effect = Instantiate(_groundeffect, _groundpoint.position, Quaternion.identity);
            Destroy(G_effect, 0.2f); // アニメーションの長さに合わせて

            Collider[] hits = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    // IDamageable 実装クラスへダメージ
                    var Interface = hit.GetComponent<IDamage>();
                    if (Interface != null) Interface.TakeDamage(_damage,_knockback,_dir);

                    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == _effectname);//ラムダ形式AIで知った
                    if (attackData != null && attackData.HitEffect != null)
                    {
                        var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
                        Destroy(effect, 0.2f);
                    }
                }
            }
            _groundattack = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}