using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,
        Move,
        Wait,
        Attack
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;
    private Rigidbody _rb;

    private void Awake()
    {
        
    }

    private void FixedUpdate()
    {
        switch( _state)
        {
            case EnemyState.Look:
                break;
            case EnemyState.Move:
                break;
            case EnemyState.Wait:
                break;
            case EnemyState.Attack:
                break;
        }
    }

    private void Look()
    {

    }

    private void Move()
    {

    }

    private void Wait()
    {

    }

    private void Attack()
    {

    }

}
