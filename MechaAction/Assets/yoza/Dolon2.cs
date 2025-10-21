using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Dolon2 : MonoBehaviour
{
    private enum EnemyState
    {
        Look,
        Move,
        Wait,
        Attack
    }

    private EnemyState _state = EnemyState.Look;
    private Rigidbody _rd;
    private Transform _player;
    private int _direction = 0;
    private float _movespeed = 3f;
    private void Awake()
    {
        _rd = GetComponent<Rigidbody>();
        _player =GameObject.FindWithTag("Player").transform;
    }
    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(_player.position, _rd.position) < 20f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                Move();
                if(Vector3.Distance(_player.position, _rd.position)>21f)
                {
                    _state = EnemyState.Look;
                }
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
        Vector3 velocity = _rd.velocity;
        if (_player.position.x < _rd.position.x) 
        {
            if (Vector3.Distance(_player.position, _rd.position) > 4f)
            {
                _direction = -1;

            }
            else if (Vector3.Distance(_player.position, _rd.position) < 2f)
            {
                _direction = 1;
            }
            velocity.x = _direction * _movespeed;
            velocity.y = 0f;
        }
        else 
        {
            if (Vector3.Distance(_player.position, _rd.position) > 4f)
            {
                _direction = 1;

            }
            else if (Vector3.Distance(_player.position, _rd.position) < 2f)
            {
                _direction = -1;
            }
            velocity.x = _direction * _movespeed;
            velocity.y = 0f;
        }

        _rd.velocity = velocity;
    }
    private void Wait()
    {

    }

    private void Attack()
    {

    }
}