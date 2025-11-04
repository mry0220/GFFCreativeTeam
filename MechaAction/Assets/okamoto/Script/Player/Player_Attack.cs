using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    //[SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] private SwordHitbox _sword;
    [SerializeField] private GunHitbox _gun;

    private Player _player;
    private int _dir;

    //private int _damage = 0;

    private enum PlayerAttackType {
        Sowd,
        Gun
    }


    private PlayerAttackType _state = PlayerAttackType.Sowd;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        //Debug.Log(_playerAttackSO.playerAttackList[0].Damage);
        //Debug.Log(_playerAttackSO.playerAttackList[1].Damage);
        _sword.enabled = false;
    }

    private void Update()
    {
        if(_player.IsDead) return;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

        switch (_state)
        {
            case PlayerAttackType.Sowd:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerAttackType.Gun;
                    //Debug.Log("GunMode");
                }

                break;
            case PlayerAttackType.Gun:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerAttackType.Sowd;
                    //Debug.Log("SowdMode");
                }

                break;
        }

        _dir = _player._lookDir;

        if(Input.GetMouseButtonDown(0))
        {
            LeftAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            tatakituke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            slash();
        }
    }

    public void LeftAttack()
    {
        //if(!_player.CanMove) return;

        _player._ChangeState(PlayerState.Attack);

        if(_state == PlayerAttackType.Sowd)
        {
            _sword.enabled = true;


            _anim.SetInteger("AttackType", 0);
            _anim.SetTrigger("Attack");
            
            _sword.leftAttack(_dir);
            //Debug.Log(_damage);
        }
        else if(_state == PlayerAttackType.Gun)
        {
            _gun.leftAttack(_dir);
            _player._ReturnNormal();
        }
    }

    public void tatakituke()
    {
        if (!_player.CanMove) return;

        if(_state == PlayerAttackType.Sowd)
        {
            _player._ChangeState(PlayerState.Attack);
            _sword.enabled = true;
            Debug.Log("反応");
            _anim.SetInteger("AttackType", 1);
            _anim.SetTrigger("Attack");
           
            _sword.tatakitukeAttack(_dir);
        }
        else if(_state == PlayerAttackType.Gun)
        {
            _gun.ShotGun(_dir);
            _player._ReturnNormal();
        }
            
    }

    public void slash()
    {
        if (!_player.CanMove) return;

        if (_state == PlayerAttackType.Sowd)
        {
            _player._ChangeState(PlayerState.Attack);
            _sword.enabled = true;

            _sword.slashAttack(_dir);
            _anim.SetTrigger("Attack");
            _anim.SetInteger("AttackType", 2);
        }
        else if (_state == PlayerAttackType.Gun)
        {
            _gun.Rifle(_dir);
            _player._ReturnNormal();
        }
       
    }

    /*private IEnumerator Enabled()
    {
        sword.enabled = true;
        yield return new WaitForSeconds(2f);
        sword.enabled = false;
        yield break;
    }*/


    public void _Enabletfalse()//animationシグナルで呼ぶ
    {
        _sword.enabled = false;
        _player._ReturnNormal();//多分呼ぶ場所がちがう
    }

    public void _Enabletrue()//animationシグナルで呼ぶ
    {
        _sword.enabled = true;
    }
}