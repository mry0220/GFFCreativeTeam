using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShieldAndFire : MonoBehaviour
{
    const float ATTACKTIME = 3f;
    const float SHIELDTIME = 3f;

    private enum EnemyState
    {
        DITECTION,
        MOVE,
        ATTACK,
        SHIELD,
        DESTROY,
    };

    private Rigidbody _rb;
    private Transform _playerTransfrom;
    private bool _ATTACK;
    private float _distance;
    private EnemyState state;
    private int _dir;
    private int tmp;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransfrom = GameObject.FindWithTag("player").transform;
    }

    private void Start()
    {
        state =EnemyState.DITECTION;
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_playerTransfrom.position, _rb.position);
        switch(state)
        {
            case EnemyState.DITECTION:
                ditection();
                break;

            case EnemyState.MOVE:
                
                break;
                
            case EnemyState.ATTACK:

                break;

            case EnemyState.SHIELD:

                break;

            case EnemyState.DESTROY:
                break;
        }
    }

    private void ditection()
    {
        if (_distance < 10)
            //state =
            move();
    }

    private void move()
    {
        tmp = _dir;

        _dir = (_rb.position.x < _playerTransfrom.position.x) ? 1 : -1;
    }

    private void attack()
    {

    }
    private void shield()
    {

    }

    private void destroy()
    {

    }
}
