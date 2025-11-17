using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,
        Move,
        Attack,
        GroundAttack
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;  // もとに戻る
    private Transform _player;
    private Rigidbody _rb;
    private ElectricEnemyAttack _attack;
    public GameObject _groundattack;

    private bool _isgrounded;
    private bool _moveStop = false;
    public int _direction = 0;

    private bool _isgroundatack = false;
    private bool _isattack = false;

    private float _lapseTime;

    private bool _isRight = false;
    private bool _isLeft = false;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<ElectricEnemyAttack>();
    }

    private void Start()
    {
        _spawnPos = transform.position;
        _lapseTime = 0.0f;
    }

    private void FixedUpdate()
    {
        _lapseTime += Time.deltaTime;

        if (_isRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _isRight = false;
        }
        else if (_isLeft)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            _isLeft = false;
        }

        switch ( _state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 10f)
                {
                    Debug.Log("発見");
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) > 12f)
                {
                    _state = EnemyState.Look;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 6f
                    && _lapseTime >= 5)
                {
                    Debug.Log("地面攻撃");
                    _state = EnemyState.GroundAttack;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 3f)
                {
                    Debug.Log("攻撃");
                    _state = EnemyState.Attack;
                }
                Move();

                break;

            case EnemyState.Attack:
                if (!_isattack)
                    StartCoroutine(Attack());
                break;

            case EnemyState.GroundAttack:
                if(!_isgroundatack)
                    StartCoroutine(GroundAttack());
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
        Debug.Log("動いてるよぅ");

        if (_moveStop)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_direction == -1 || _direction == 0)
            {
                StartCoroutine(Waitturn(1));
                _isRight = true;
            }
            _direction = 1;
        }
        else
        {
            if (_direction == 1 || _direction == 0)
            {
                StartCoroutine(Waitturn(-1));
                _isLeft = true;
            }
            _direction = -1;
        }
        velocity.x = _direction * 3f;

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

    private IEnumerator Attack()
    {
        _isattack = true;
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        _attack.ElectricAttack();
        _lapseTime = 0;

        _state = EnemyState.Move;
        _isattack = false;
        yield break;
    }

    private IEnumerator GroundAttack()
    {
        _isgroundatack = true;
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2f);
        //インスタンス化
        GameObject _attack = Instantiate(_groundattack, transform.position, Quaternion.identity);
        _attack.GetComponent<GroundBall>().Initialize(this);
        Debug.Log("deta");
        _lapseTime = 0;

        _state = EnemyState.Move;
        _isgroundatack = false;
        yield break;
    }

}
