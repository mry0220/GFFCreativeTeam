using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] PlayerAttackSO _playerAttackSO;
    [SerializeField] private SwordHitbox sword;

    //private int _damage = 0;

    private enum PlayerState {
        Sowd,
        Gun
    }


    private PlayerState _state = PlayerState.Sowd;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        //Debug.Log(_playerAttackSO.playerAttackList[0].Damage);
        //Debug.Log(_playerAttackSO.playerAttackList[1].Damage);

    }

    private void Update()
    {
        switch(_state)
        {
            case PlayerState.Sowd:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerState.Gun;
                    //Debug.Log("GunMode");
                }

                break;
            case PlayerState.Gun:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerState.Sowd;
                    //Debug.Log("SowdMode");
                }

                break;
        }

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
        if(_state == PlayerState.Sowd)
        {
            _anim.SetTrigger("Attack");
            _anim.SetInteger("Attacktype", 0);


            //Debug.Log(_damage);
        }
        else if(_state == PlayerState.Gun)
        {
           

        }
    }

    public void tatakituke()
    {
        sword.enabled = true;
        //StartCoroutine(Enabled());

        //_damage = _playerAttackSO.playerAttackList[0].Damage;
        //Debug.Log(_damage);
        //_anim.SetTrigger("Attack");
        //_anim.SetInteger("Attacktype", 1);
        sword.tatakitukeAttack();
        _anim.SetTrigger("Attack");
        //_anim.SetInteger("Attacktype", 0);
    }

    public void slash()
    {
        sword.enabled = true;
        //StartCoroutine(Enabled());
        //_damage = _playerAttackSO.playerAttackList[1].Damage;

        //Debug.Log(_damage);
        sword.slashAttack();
        _anim.SetTrigger("Attack");
        //_anim.SetInteger("Attacktype", 0);
    }

    /*private IEnumerator Enabled()
    {
        sword.enabled = true;
        yield return new WaitForSeconds(2f);
        sword.enabled = false;
        yield break;
    }*/

    public void OnEnable()//animationƒVƒOƒiƒ‹‚ÅŒÄ‚Ô
    {
        sword.enabled = false;
    }
}