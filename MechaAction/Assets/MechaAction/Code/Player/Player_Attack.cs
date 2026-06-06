using Critical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private GameObject m_swordObj;
    [SerializeField] private GameObject m_handGunObj;
    [SerializeField] private GameObject m_shotGunObj;


    private Animator _anim;
    [SerializeField] private PlayerT m_player;
    private DirectionTarget _dirtarget;

    [Header("UI")]
    [SerializeField] private GameObject ammoIconPrefab;

    [Header("EasyCommandCheck")]
    [SerializeField] private BoolRunTimeSO m_IsCommandCheck;

    private Transform ammoParent;
    private int _currentammo;
    private int _Maxammo = 10;
    private List<GameObject> ammoIcons = new List<GameObject>();

    private float _skillMax = 100;
    private float _currectskill = 20;
    private float _skilltime;

    public float SkillMax => _skillMax;

    public float Currentskill => _currectskill;

    //private CoolDown coolDown = new CoolDown();

    //private Coroutine _tatakitukecoroutine;
    //private Coroutine _shotguncoroutine;
    //private Coroutine _slashcoroutine;
    //private Coroutine _riflecoroutine;

    private enum PlayerAttackType {
        Sowd,
        Gun
    }

    private PlayerAttackType m_state = PlayerAttackType.Sowd;

    [SerializeField] private FloatEvent m_skillevent;
    private CriticalDamage m_criticaldamage = new CriticalDamage();

    private int m_damage;
    private float m_criticalRate;
    private float m_criticalChance;
    private int m_knockback;
    private AttackType m_type;
    private float m_duration;

    //private float _SKILL = 0f;

    private void OnEnable()
    {
        m_skillevent.Register(Skillgauge);
    }

    private void OnDisable()
    {
        m_skillevent.Unregister(Skillgauge);
    }

    public void Skillgauge(float gauge)//敵を倒したら回復
    {
        _currectskill += gauge;
    }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _dirtarget = GetComponent<DirectionTarget>();

        ammoParent = GameObject.Find("AmmoParent")?.transform;
    }

    private void Start()
    {
        ApplySkillUpgrades();

        _currentammo = _Maxammo;

        // 初期化
        for (int i = 0; i < _Maxammo; i++)
        {
            GameObject icon = Instantiate(ammoIconPrefab, ammoParent);
            ammoIcons.Add(icon);
        }
    }

    private void ApplySkillUpgrades()
    {
        //if (SkillManager.Instance.HasSkill(SkillType.HP1))
        //{
        //    _SKILL += 0.1f;
        //    Debug.Log("スキルアップ！");
        //}
        //if (SkillManager.Instance.HasSkill(SkillType.HP2))
        //{
        //    _SKILL += 0.2f;
        //    Debug.Log("スキルアップ！");
        //}
        //if (SkillManager.Instance.HasSkill(SkillType.HP3))
        //{
        //    _SKILL += 0.3f;
        //    Debug.Log("スキルアップ！");
        //}
    }

   

    private void Update()
    {
        if(m_player.IsDead) return;

        switch (m_state)
        {
            case PlayerAttackType.Sowd:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    m_state = PlayerAttackType.Gun;
                    m_handGunObj.SetActive(true);
                    m_swordObj.SetActive(false);
                    //Debug.Log("GunMode");
                }

                break;
            case PlayerAttackType.Gun:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    m_state = PlayerAttackType.Sowd;
                    m_swordObj.SetActive(true);
                    m_handGunObj.SetActive(false);
                    m_shotGunObj.SetActive(false);
                    //Debug.Log("SowdMode");
                }

                break;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(_dirtarget._ischeck == false)
                _dirtarget._ischeck = true;
            else
                _dirtarget._ischeck = false;
        }

        if (m_IsCommandCheck.Value)//Optionで変更
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    LeftAttack();
            //}


            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_currectskill < 10) return;
                _currectskill -= 10;
                tatakituke();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_currectskill < 10) return;
                _currectskill -= 10;
                slash();
            }
        }

        _skilltime += Time.deltaTime;
        if(_skilltime > 0.1)
        {
            _skilltime = 0;
            _currectskill += 5f;
        }

    }
    public void CallLeftAttack()
    {
        LeftAttack();
    }

    public void Calltatakituke()
    {
        if (_currectskill < 10) return;
        _currectskill -= 10;
        tatakituke();
    }

    public void CallSlash()
    {
        if (_currectskill < 10) return;
        _currectskill -= 10;
        slash();
    }

    public void LeftAttack()
    {
        if(!m_player.CanMove) return;

        m_player.ChangeState(PlayerState.Attack);

        if(m_state == PlayerAttackType.Sowd)
        {
            _anim.SetInteger("AttackType", 0);
            _anim.SetTrigger("Attack");
            DefaultAttack();
        }
        else if(m_state == PlayerAttackType.Gun)
        {
            m_handGunObj.SetActive(true);
            m_shotGunObj.SetActive(false);

            if (_currentammo > 0)
            {
                _anim.SetInteger("AttackType", 3);
                _anim.SetTrigger("Attack");
                DefaultGun();
                _currentammo -= 1;
                // 右端の弾を消す
                ammoIcons[_currentammo].SetActive(false);
            }

            m_player._ReturnNormal();//シグナルで呼べないから
        }
    }

    public void Relod()
    {
        if(m_state == PlayerAttackType.Gun)
        {
            _currentammo = _Maxammo;

            foreach (var icon in ammoIcons)
            {
                icon.SetActive(true);
            }
        }
    }

    public void tatakituke()
    {
        if (!m_player.CanMove) return;
        m_player.ChangeState(PlayerState.Attack);

        if (m_state == PlayerAttackType.Sowd)
        {
            _anim.SetInteger("AttackType", 1);
            _anim.SetTrigger("Attack");

            //Animation Event
        }
        else if(m_state == PlayerAttackType.Gun)
        {
            m_handGunObj.SetActive(false);
            m_shotGunObj.SetActive(true);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            ShotGun();

            m_player._ReturnNormal();//シグナルで呼べないから
        }
    }


    //if (!_player.CanMove || _tatakitukecoroutine != null) return;

    //_tatakitukecoroutine = StartCoroutine(
    //    coolDown.Skill(callback => { _tatakitukecoroutine = callback; },
    //    cooltime,
    //    null,
    //    _sword.tatakitukeAttack,
    //    0,
    //    _dir));
    //Debug.Log((int)skill.Current);

    public void slash()
    {
        if (!m_player.CanMove) return;
        m_player.ChangeState(PlayerState.Attack);

        if (m_state == PlayerAttackType.Sowd)
        {
            _anim.SetTrigger("Attack");
            _anim.SetInteger("AttackType", 2);

            ElectAttack();
        }
        else if (m_state == PlayerAttackType.Gun)
        {
            m_handGunObj.SetActive(false);
            m_shotGunObj.SetActive(true);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            RifleGun();

            m_player._ReturnNormal();//シグナルで呼べないから
        }
    }

    public void DataApply(AttackDataSO data)
    {
        m_damage = data.Damage;
        m_criticalRate = data.CriticalRate;
        m_criticalChance = data.CriticalChance;
        m_knockback = data.Knockback;
        m_type = data.Type;
        m_duration = data.Duration;
    }

    //=============== Swod Attack =================
    [Header("DefaultAttack")]
    [SerializeField] private HitCollider m_DAttack;
    [SerializeField] private AttackDataSO m_DAttackData;

    public void DefaultAttack()
    {
        DataApply(m_DAttackData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        m_DAttack.AttackCollider(data, m_player.Team);
        //effect.Play
    }


    [Header("ElectAttack")]
    [SerializeField] private AttackDataSO m_EAttackData;

    public void ElectAttack()
    {
        DataApply(m_EAttackData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        //攻撃オブジェクトに関数を渡す
        //effect.Play(プレイヤーについたエフェクトオブジェクトを動かす
    }


    [Header("GroundAttack")]
    [SerializeField] private HitCollider m_GAttack;
    [SerializeField] private AttackDataSO m_GAttackData;

    public void GroundAttack()
    {
        DataApply(m_GAttackData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        m_GAttack.AttackCollider(data, m_player.Team);
        //effect.Play
    }//Animation Event


    //=============== Gun Attack =================
    [Header("DefaultGun")]
    [SerializeField] private HitRay m_DGun;
    [SerializeField] private AttackDataSO m_DGunData;

    public void DefaultGun()
    {
        DataApply(m_DGunData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        m_DGun.AttackCast(data, m_player.Team);
        //effect.Play
    }


    [Header("ShotGun")]
    [SerializeField] private HitRay m_SGun;
    [SerializeField] private AttackDataSO m_SGunData;

    public void ShotGun()
    {
        DataApply(m_SGunData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        m_SGun.AttackCastPenetration(data, m_player.Team);
        //effect.Play
    }


    [Header("RifleGun")]
    [SerializeField] private HitRay m_RGun;
    [SerializeField] private AttackDataSO m_RGunData;

    public void RifleGun()
    {
        DataApply(m_RGunData);

        bool iscritical = false;
        iscritical = m_criticaldamage.IsCritical(ref iscritical, m_criticalChance);

        if (iscritical) Debug.Log("クリティカル!");

        DamageData data = new DamageData
        {
            damage = m_damage,
            isCritical = iscritical,
            criticalRate = m_criticalRate,
            knockback = m_knockback,
            type = m_type,
            duration = m_duration,
            attackDir = m_player.Forward
        };

        m_RGun.AttackCastPenetration(data, m_player.Team);
        //effect.Play
    }

}