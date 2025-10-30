using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] private SwordHitbox sword;

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

    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.R))
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
            _anim.SetTrigger("Attack");
            _anim.SetInteger("AttackType", 0);

            sword.leftAttack(_dir);
            //Debug.Log(_damage);
        }
        else if(_state == PlayerAttackType.Gun)
        {
           

        }
    }

    public void tatakituke()
    {
        if (!_player.CanMove) return;

        _player._ChangeState(PlayerState.Attack);
        sword.enabled = true;

        _anim.SetTrigger("Attack");
        _anim.SetInteger("AttackType", 1);
        sword.tatakitukeAttack(_dir);
    }

    public void slash()
    {
        if (!_player.CanMove) return;

        _player._ChangeState(PlayerState.Attack);
        sword.enabled = true;

        sword.slashAttack(_dir);
        _anim.SetTrigger("Attack");
        _anim.SetInteger("AttackType", 2);
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
        sword.enabled = false;
        _player._ReturnNormal();//多分呼ぶ場所がちがう
    }

    public void _Enabletrue()//animationシグナルで呼ぶ
    {
        sword.enabled = true;
    }
}