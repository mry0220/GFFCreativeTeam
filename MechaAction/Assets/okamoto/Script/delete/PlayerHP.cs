using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour ,IPlayerDamage
{
    private Player _player;
    [SerializeField] DamageEffectSO _damageEffectSO;

    private int maxHP = 100;
    private int currentHP;

    // 現在のHPを他のスクリプト（UIなど）から取得するための公開プロパティ
    public int CurrentHP => currentHP; //UIHPゲージで使う　ゲームマネージャーでmaxにするためだめかも
    public int MaxHP => maxHP;//こっちもまあ使うか


    public void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        currentHP = maxHP;
        Debug.Log(gameObject.name + "のHPが初期化されました: " + currentHP);
    }


    //ダメージを受け、HPを減少させる。
    public void TakeDamage(int damage, int knockback, int dir, string name)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);


        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Debug.Log(gameObject.name + "が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        if (damage >= 10)
        {
            if (knockback >= 0 && knockback < 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.SKnockBack(dir, knockback);
                StartCoroutine(_DamageTime(0.5f));
                StartCoroutine(_StateNormal(0.5f));
            }
            if (knockback >= 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.BKnockBack(dir, knockback);
                StartCoroutine(_DamageTime(1f));
                StartCoroutine(_StateNormal(1f));
            }
        }
        // HPが0以下になったかチェック
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void TakeElectDamage(int damage, float electtime, string name)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, electtime);
        }

        Debug.Log(gameObject.name + "が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        _player._ChangeState(PlayerState.Other);
        _player.Stun();//vector3.zero
        StartCoroutine(_StateNormal(electtime));

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void TakeBanDamage(float bantime, string name)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, bantime);
        }

        StartCoroutine(_BanTime(bantime));
    }

    private IEnumerator _StateNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _player._ReturnNormal();
        yield break;
    }

    private IEnumerator _BanTime(float bantime)//ジャンプ制限
    {
        _player._isBan = true;
        yield return new WaitForSeconds(bantime);
        _player._isBan = false;
        yield break;
    }

    private IEnumerator _DamageTime(float time)
    {
        GManager.Instance.OnPlayerHit();//カメラ揺らす

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(time);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), false);

        yield break;
    }

    // HPを回復させる。
    public void Heal(int healAmount)
    {
        currentHP += healAmount;

        currentHP = Mathf.Min(currentHP, maxHP);

        Debug.Log(gameObject.name + "のHPが" + healAmount + "回復しました。残りHP: " + currentHP);
    }


    // HPが0になったときの死亡処理。（プレイヤー専用）
    public void Die()
    {
        Debug.Log(gameObject.name + "は倒されました。ゲームオーバー！");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        _player._ChangeState(PlayerState.Dead);
        _player.Dead();//animとか
        GManager.Instance.DiePlayer();

        // ここにプレイヤー入力の無効化などの処理を追加
        // プレイヤーオブジェクトを非アクティブ化
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeadArea"))//落下死亡isTrigger
        {
            GManager.Instance.OnPlayerHit();//カメラ揺らす
            currentHP = 0;
            Die();
        }
    }

    public IEnumerator ResetHP()//GManagerから復活の命令
    {
        currentHP = MaxHP;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
           LayerMask.NameToLayer("Enemy"), false);
        yield return new WaitForSeconds(1f);
        _player.Dead();//Vector3.zeroにするため　後消す
        _player._ChangeState(PlayerState.Respawn);//一旦respawnに
        StartCoroutine(_StateNormal(1f));//standingにもどす
        yield break ;
    }
}
