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

        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 13f)
                {
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                Move();
                if (Vector3.Distance(transform.position, _player.position) > 17f)
                {
                    _state = EnemyState.Look;
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

        //FrontJump();
        //BackJump();

        _rb.velocity = velocity;
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

    }
}
