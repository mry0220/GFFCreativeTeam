
using Critical;
using TMPro;
using UnityEngine;
public class EnemyHP : MonoBehaviour,IDamage
{
    [SerializeField] private int maxHP; // HP
    [SerializeField] private float skillgauge;
    [SerializeField] private float score;
    [SerializeField] private GameObject _recover;
    [SerializeField] DamageEffectSO _damageEffectSO;
    private IEnemy _ienemy;

    private int _clear;
    private Transform _player;
    private CriticalDamage _criticaldamage = new CriticalDamage();
    private int currentHP;
    //[SerializeField] private float currentHealth; // デバッグ用にInspectorで確認可能になる

    //[SerializeField]
    //private GameObject _targets;
    private GameObject _canvas;

    [SerializeField]
    private GameObject _damageUI;
    [SerializeField]
    private GameObject _criticaldamageUI;

    // IHealthインターフェースの実装（読み取り専用プロパティ）
    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _ienemy = GetComponent<IEnemy>();
        _canvas = GameObject.Find("Canvas");
    }

    void Start()
    {
        _clear = GManager.Instance.clear;
        maxHP = maxHP + (_clear * 50);
        currentHP = maxHP;
        //Debug.Log("<color=red>" + gameObject.name + " (敵) のHPが初期化されました: " + currentHP);
    }

    private void Update()
    {
        DistanceDead();
    }

    public void TakeDamage(DamageData data,DamageResult result)
    {

    }

    //ダメージを受け、HPを減少させる処理
    //public void TakeDamage(int damage, int knockback, int dir, string audioname)
    //{
    //    if (currentHP <= 0) return;
    //    bool iscritical = false;
    //    //AudioManager.Instance.PlaySound(audioname);
    //    //クリティカル
    //    damage = _criticaldamage.damage(ref iscritical, damage,50f);
    //    if (iscritical)
    //    {
    //        Debug.Log("クリティカル!");
    //        CriticalDamageUI(damage);
    //    }
    //    else
    //    {
    //        PopDamageUI(damage);
    //    }

    //    currentHP -= damage;
    //    currentHP = Mathf.Max(currentHP, 0); // 0未満にならないようにクランプ
    //    GManager.Instance.OnPlayerHit();

    //    if (damage >= 10)
    //    {
    //        if (knockback >= 0 && knockback < 5)
    //        {
    //            _ienemy.SKnockBack(dir, knockback);
    //        }
    //        if (knockback >= 5)
    //        {
    //            Debug.Log("Bkock");
    //            _ienemy.BKnockBack(dir, knockback);
    //        }
    //    }

    //    Debug.Log("<color=blue>" + gameObject.name + " (敵) が" + damage + "ダメージ受けました。残りHP: " + currentHP);

    //    if (currentHP <= 0)
    //    {
    //        Die();
    //    }
    //}

    //public void TakeElectDamage(int damage,int knockback,int dir, float electtime, string audioname)
    //{
    //    if (currentHP <= 0) return;

    //    //AudioManager.Instance.PlaySound(audioname);

    //    currentHP -= damage;
    //    currentHP = Mathf.Max(currentHP, 0); // 0未満にならないようにクランプ
    //    GManager.Instance.OnPlayerHit();

    //    _ienemy.ElectStun(dir, knockback, electtime);
  

    //    var attackData = _damageEffectSO.damageEffectList.Find(x => x.EffectName == "DamageEffect");//ラムダ形式AIで知った
    //    if (attackData != null && attackData.HitEffect != null)
    //    {
    //        var effect = Instantiate(attackData.HitEffect, transform.position, Quaternion.identity);
    //        Destroy(effect, 0.2f);
    //    }

    //    Debug.Log("<color=blue>" + gameObject.name + " (敵) が" + damage + "ダメージ受けました。残りHP: " + currentHP);

    //    if (currentHP <= 0)
    //    {
    //        Die();
    //    }
    //}

    // HPを回復させる処理 (敵にはあまり使わないかもしれないが、インターフェースの契約として実装)
    //public void Heal(int amount)
    //{
    //    currentHP += amount;
    //    currentHP = Mathf.Min(currentHP, maxHP); // 最大HPを超えないようにクランプ

    //    Debug.Log(gameObject.name + " (敵) のHPが" + amount + "回復しました。残りHP: " + currentHP);
    //}


    private void DistanceDead()
    {
        if (Vector3.Distance(transform.position, _player.position) > 30f)
        {
            Destroy(gameObject);
        }
    }

    // 敵独自の死亡処理
    public void Die()
    {
        Debug.Log("<color=blue>" + gameObject.name + " (敵) は倒されました！");
        GManager.Instance.SkillGauge(skillgauge);
        GManager.Instance.ScoreUP(score);
        int number = Random.Range(0, 100);
        if(number >= 0&&number < 30)
        {
            var recover = Instantiate(_recover, transform.position, Quaternion.identity);
        }
        
        // 独自の敵の死亡処理を記述
        // 例1: スコアを加算する処理
        // GameManager.Instance.AddScore(100); 

        // 例2: 爆発エフェクトを再生する処理
        // Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 例3: オブジェクトをシーンから破壊
        Destroy(gameObject);
    }

    private void PopDamageUI(int damage)
    {
        var obj = new GameObject("Target");
        var ui = Instantiate(_damageUI);

        //obj.transform.SetParent(_targets.transform);
        ui.transform.SetParent(_canvas.transform);

        UIPosControl pos = ui.GetComponent<UIPosControl>();
        pos.SetDamage(damage);

        //pos.Initialize(transform.position + Vector3.up * 1.5f);

        //null検知
        ui.GetComponent<UIPosControl>().target = obj.transform;

        //ui.transform.GetComponent<TextMeshPro>().text = damage.ToString();
        ui.SetActive(true);

        var circlePos = Random.insideUnitCircle * 1.2f;
        obj.transform.position = transform.position + Vector3.up * Random.Range(3.0f, 4.0f) + new Vector3(circlePos.x, 0, circlePos.y);
        ui.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);

        Destroy(obj, 2.0f);
        Destroy(ui, 2.0f);
    }

    private void CriticalDamageUI(int damage)
    {
        var obj = new GameObject("Target");
        var ui = Instantiate(_criticaldamageUI);

        //obj.transform.SetParent(_targets.transform);
        ui.transform.SetParent(_canvas.transform);

        UIPosControl pos = ui.GetComponent<UIPosControl>();
        pos.SetDamage(damage);

        //pos.Initialize(transform.position + Vector3.up * 1.5f);

        //null検知
        ui.GetComponent<UIPosControl>().target = obj.transform;

        //ui.transform.GetComponent<TextMeshPro>().text = damage.ToString();
        ui.SetActive(true);

        var circlePos = Random.insideUnitCircle * 1.2f;
        obj.transform.position = transform.position + Vector3.up * Random.Range(3.0f, 4.0f) + new Vector3(circlePos.x, 0, circlePos.y);
        ui.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, obj.transform.position);

        Destroy(obj, 2.0f);
        Destroy(ui, 2.0f);
    }
}
