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
    private float _moveSpeed = 1.0f;

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
                else if (Vector3.Distance(transform.position, _player.position) < 1f)
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
        //int dire

        //if(transform.position < _player.position)

        Vector3 direction = (_player.position - _rb.position).normalized;

        Vector3 velocity = _rb.velocity;

        velocity.x = direction.x * _moveSpeed;

        _rb.velocity = velocity;

    }

    private void Wait()
    {

    }

    private void Attack()
    {
        Debug.Log("Attack");
    }
}
