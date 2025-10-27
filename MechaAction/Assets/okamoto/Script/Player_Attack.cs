using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private enum PlayerState {
        Sowd,
        Gun
    }

    private PlayerState _state = PlayerState.Sowd;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        switch(_state)
        {
            case PlayerState.Sowd:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerState.Gun;
                    Debug.Log("GunMode");
                }

                break;
            case PlayerState.Gun:
                if (Input.GetKeyDown(KeyCode.G))
                {
                    _state = PlayerState.Sowd;
                    Debug.Log("SowdMode");
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
    }

    public void LeftAttack()
    {
        if(_state == PlayerState.Sowd)
        {
            _anim.SetTrigger("Attack");
            _anim.SetInteger("Attacktype", 0);

        }
        else if(_state == PlayerState.Gun)
        {


        }
    }

    public void tatakituke()
    {
        _anim.SetTrigger("Attack");
        _anim.SetInteger("Attacktype", 1);
    }
}
/*
#include 
int a = 2004;

int main(void)
{
    printf("%d”N%dŒŽ%d“ú",a,04,9);
}*/
