using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cooltime;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private GameObject Sword;
    [SerializeField] private GameObject HandGun;
    [SerializeField] private GameObject ShotGun;
    [SerializeField] private SwordHitbox _sword;
    [SerializeField] private GunHitbox _gun;

    private Animator _anim;
    [SerializeField] private Player m_player;
    private DirectionTarget _dirtarget;
    private int _dir;

    [Header("UI")]
    [SerializeField] private GameObject ammoIconPrefab;

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

    private float _SKILL = 0f;

    private void OnEnable()
    {
        m_skillevent.Register(Skillgauge);
    }

    private void OnDisable()
    {
        m_skillevent.Unregister(Skillgauge);
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
        if (SkillManager.Instance.HasSkill(SkillType.HP1))
        {
            _SKILL += 0.1f;
            Debug.Log("スキルアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP2))
        {
            _SKILL += 0.2f;
            Debug.Log("スキルアップ！");
        }
        if (SkillManager.Instance.HasSkill(SkillType.HP3))
        {
            _SKILL += 0.3f;
            Debug.Log("スキルアップ！");
        }
    }

    public void Skillgauge(float gauge)//敵を倒したら回復
    {
        _currectskill += gauge;
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
                    HandGun.SetActive(true);
                    Sword.SetActive(false);
                    //Debug.Log("GunMode");
                }

                break;
            case PlayerAttackType.Gun:
                if (Input.GetKeyDown(KeyCode.F))
                {
                    m_state = PlayerAttackType.Sowd;
                    Sword.SetActive(true);
                    HandGun.SetActive(false);
                    ShotGun.SetActive(false);
                    //Debug.Log("SowdMode");
                }

                break;
        }

        _dir = m_player.LookDir;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if(_dirtarget._ischeck == false)
                _dirtarget._ischeck = true;
            else
                _dirtarget._ischeck = false;
        }

        if (GManager.Instance.IsCommandEasy)//Optionで変更
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
            _sword.leftAttack(_dir);
        }
        else if(m_state == PlayerAttackType.Gun)
        {
            HandGun.SetActive(true);
            ShotGun.SetActive(false);

            if (_currentammo > 0)
            {
                _anim.SetInteger("AttackType", 3);
                _anim.SetTrigger("Attack");
                _gun.leftAttack(_dir);
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
         
            _sword.tatakitukeAttack(_dir);
        }
        else if(m_state == PlayerAttackType.Gun)
        {
            HandGun.SetActive(false);
            ShotGun.SetActive(true);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            _gun.ShotGun(_dir);

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
            _sword.enabled = true;
            _anim.SetTrigger("Attack");
            _anim.SetInteger("AttackType", 2);

            _sword.slashAttack(_dir);
        }
        else if (m_state == PlayerAttackType.Gun)
        {
            HandGun.SetActive(false);
            ShotGun.SetActive(true);

            _anim.SetInteger("AttackType", 3);
            _anim.SetTrigger("Attack");

            _gun.Rifle(_dir);

            m_player._ReturnNormal();//シグナルで呼べないから

        }
    }

    public void _Enabletfalse()//animationシグナルで呼ぶ
    {
        m_player._ReturnNormal();//最後に呼ぶ
    }

    [SerializeField] private HitCollider m_GAttack;

    public void GroundAttack()
    {
        DamageData data = new DamageData
        {
            damage = 0
        };

        m_GAttack.AttackCollider(data,m_player.Team,m_player.Forward);
    }

}