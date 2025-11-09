using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP_T : MonoBehaviour ,IPlayerDamage
{
    private Player _player;
    [SerializeField] DamageEffectSO _damageEffectSO;


    // [SerializeField] でInspectorから設定可能にする
    [SerializeField] private int maxHP = 100;

    // 現在のHPはprivate変数で管理
    private int currentHP;

    // 現在のHPを他のスクリプト（UIなど）から取得するための公開プロパティ
    public int CurrentHP => currentHP; //UIHPゲージで使う　ゲームマネージャーでmaxにするためだめかも
    public int MaxHP => maxHP;//こっちもまあ使うか

    void Start()
    {
        // ゲーム開始時にHPを最大値に設定
        currentHP = maxHP;
        Debug.Log(gameObject.name + "のHPが初期化されました: " + currentHP);
        _player = GetComponent<Player>();
    }


    //ダメージを受け、HPを減少させる。
    public void TakeDamage(int damage,int knockback,int dir,string name)
    {
        if (currentHP <= 0)
        {
            // 既に死亡している場合は処理をスキップ
            return;
        }
        // --- ここからダメージ処理 ---
        currentHP -= damage;
        // HPが0未満にならないように制限
        currentHP = Mathf.Max(currentHP, 0);
        StartCoroutine(_DamageTime());

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        Debug.Log(gameObject.name + "が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        if(damage >= 10)
        {
            if(knockback  >= 0 && knockback < 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.SKnockBack(dir, knockback);
                StartCoroutine(_StateNormal(0.5f));
            }
            if (knockback >= 5)
            {
                _player._ChangeState(PlayerState.Knockback);
                _player.BKnockBack(dir, knockback);
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
        // --- ここからダメージ処理 ---
        currentHP -= damage;
        // HPが0未満にならないように制限
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(gameObject.name + "が" + damage + "ダメージ受けました。残りHP: " + currentHP);

        _player._ChangeState(PlayerState.Other);
        _player.Stun();//vector3.zero
        StartCoroutine(_StateNormal(electtime));

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, electtime);
        }

        // HPが0以下になったかチェック
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

        StartCoroutine(_BanTime(bantime));

        var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == name);//ラムダ形式AIで知った
        if (attackData != null && attackData.HitEffect != null)
        {
            var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
            Destroy(effect, bantime);
        }

    }

    private IEnumerator _StateNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _player._ReturnNormal();
        yield break;
    }

    private IEnumerator _BanTime(float bantime)
    {
        _player._isBan = true;
        yield return new WaitForSeconds(bantime);
        _player._isBan = false;
        yield break;
    }

    private IEnumerator _DamageTime()
    {
        GManager.Instance.OnPlayerHit();
        Debug.Log("無敵");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), true);
        for (int i= 0; i < 10; i++)
        {
            

        }
        yield return new WaitForSeconds(3f);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"), false);

        yield break;
    }

    // HPを回復させる。
    public void Heal(int healAmount)
    {
        currentHP += healAmount;

        // HPが最大値を超えないように制限
        currentHP = Mathf.Min(currentHP, maxHP);

        Debug.Log(gameObject.name + "のHPが" + healAmount + "回復しました。残りHP: " + currentHP);
    }


    // HPが0になったときの死亡処理。（プレイヤー専用）
    public void Die()
    {
        Debug.Log(gameObject.name + "は倒されました。ゲームオーバー！");
        int layer = LayerMask.NameToLayer("PlayerDamage");
        gameObject.layer = layer;
        _player._ChangeState(PlayerState.Dead);
        _player.Dead();
        GManager.Instance.DiePlayer();

        // ここにプレイヤー入力の無効化などの処理を追加
        // プレイヤーオブジェクトを非アクティブ化
        //gameObject.SetActive(false);
    }
}
