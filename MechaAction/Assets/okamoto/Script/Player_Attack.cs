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
    }

    public void LeftAttack()
    {
        if(_state == PlayerState.Sowd)
        {


        }
        else if(_state == PlayerState.Gun)
        {


        }
    }

    public void tatakituke()
    {

    }
}
/*
#include 
int a = 2004;

int main(void)
{
    printf("%d”N%dŒŽ%d“ú",a,04,9);
}*/
