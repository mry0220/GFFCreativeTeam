using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DogEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,          //íTÇ∑
        Move,          //í«ê’
        Wait,          //î≠éÀópà”(Ç¢ÇÁÇ»Ç¢)
        Attack         //î≠éÀ
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;//Ç‡Ç∆Ç…Ç‡Ç«ÇÈÇΩÇﬂ
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
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 8f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                Move();
                if (Vector3.Distance(transform.position, _player.position) > 9f)
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
        
    }

    private void Wait()
    {

    }

    private void Attack()
    {

    }
}
