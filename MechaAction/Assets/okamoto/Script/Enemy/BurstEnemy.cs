using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
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
    private float _moveSpeed = 3.0f;
    private bool _moveStop = false;
    private int _direction = 0;
    private float _attacktime = 0;

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
                _attacktime = 0.0f;
                if (Vector3.Distance(transform.position, _player.position) < 8f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                Move();
                _attacktime = Time.fixedDeltaTime;
                if (Vector3.Distance(transform.position, _player.position) > 9f)
                {
                    _state = EnemyState.Look;
                }
                else if ((_attacktime / 10) == 0)
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
        _direction = 0;
    }

    private void Move()
    {
        Vector3 velocity = _rb.velocity;

        if (_moveStop)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if(_direction == -1)
            {
                StartCoroutine(Waitturn(1));
            }
            _direction = 1;
        }
        else
        {
            if (_direction == 1)
            {
                StartCoroutine(Waitturn(-1));
            }
            _direction = -1;
        }

        //int direction = (_rb.position.x < _player.position.x) ? 1 : -1;

        //Vector3 direction = (_player.position - _rb.position).normalized;

        velocity.x = _direction * _moveSpeed;

        _rb.velocity = velocity;
        Debug.Log(_direction);
    }

    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _direction = _newdirection;
        _moveStop = false;

        yield break;
    }

    private void Wait()
    {

    }

    private void Attack()
    {
        Debug.Log("Attack");
    }
}
