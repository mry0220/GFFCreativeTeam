using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BurstEnemy : MonoBehaviour
{
    private enum EnemyState { 
        Look,          //íTÇ∑
        Move,          //í«ê’
        Wait,          //î≠éÀópà”
        Attack         //î≠éÀ
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;
    private Transform _player;
    private Rigidbody _rb;

    private void Awake()
    {
        _spawnPos = transform.position;
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch(_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 5f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                Move();
                if (Vector3.Distance(transform.position, _player.position) > 6f)
                {
                    _state = EnemyState.Look;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 4f)
                {
                    _state = EnemyState.Attack;
                }
                break;

            case EnemyState.Wait:
                Wait();
                break;

            case EnemyState.Attack:
                Attack();
                _state = EnemyState.Move;
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
        Debug.Log("Attack");
    }
}
