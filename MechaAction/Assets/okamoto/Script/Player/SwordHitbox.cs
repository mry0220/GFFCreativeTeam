using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    //1回の攻撃で同じ敵に何度も当たり判定が入らないようにする
    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();
    //public int damage = 10;//プレイヤースクリプトで技によって変更
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] GameObject _slashPrefab;
    public Transform _slashPosition;
    private int _damage;
    private int _knockback;
    private int _dir = 0;

    private void Awake()
    {

    }

    private void Start()
    {
        this.enabled = false;
    }

    void OnEnable()
    {
        // 攻撃開始時にリストをリセット
        hitTargets.Clear();
    }

    private void Update()
    {
        
    }

    public void leftAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[0].Damage;
        _knockback = _playerAttackSO.playerAttackList[0].Knockback;
        _dir = dir;
    }

    //問題　連続した攻撃の際、アニメーションのシグナルか何かでClear();を呼ぶ必要あり

    public void tatakitukeAttack(int dir)
    {
        Debug.Log("叩きつけ");
        _damage = _playerAttackSO.playerAttackList[1].Damage;
        _knockback = _playerAttackSO.playerAttackList[1].Knockback;
        _dir = dir;
    }

    public void slashAttack(int dir)
    {
        _damage = _playerAttackSO.playerAttackList[2].Damage;
        _knockback = _playerAttackSO.playerAttackList[2].Knockback;
        _dir += dir;

        Instantiate(_slashPrefab, _slashPosition.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("当たった");

        if (other.CompareTag("Enemy") && !hitTargets.Contains(other.gameObject))
        {
            //null条件

            other.GetComponent<IDamage>().TakeDamage(_damage,_knockback,_dir);//敵のインターフェース<IDamage>取得
            hitTargets.Add(other.gameObject);
        }
    }
}