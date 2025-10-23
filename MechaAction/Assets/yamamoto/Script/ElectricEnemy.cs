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

    private Vector3 _spawnPos;  // ‚à‚Æ‚É–ß‚é
    private Transform _player;
    private Rigidbody _rb;

    private bool _isgrounded;
    private bool _moveStop = false;
    private int _direction = 0;

    

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _spawnPos = transform.position;
    }

    private void FixedUpdate()
    {
        switch( _state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 10f)
                {
                    Debug.Log("”­Œ©");
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) > 12f)
                {
                    _state = EnemyState.Look;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 7f)
                {
                    Debug.Log("’n–ÊUŒ‚");
                    _state = EnemyState.Attack;
                }
                    Move();

                break;

            case EnemyState.Wait:
                break;

            case EnemyState.Attack:

                break;
        }
    }

    private void Look()
    {
        _direction = 0;
    }

    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);
        _direction = _newdirection;
        _moveStop = false;
        yield break;
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
            if (_direction == -1)
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
        velocity.x = _direction * 3f;

        _rb.velocity = velocity;
    }

    private void Wait()
    {

    }

    private void Attack()
    {

    }

}
