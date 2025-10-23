using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DogEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,          //探す
        Move,          //追跡
        Wait,          //発射用意(いらない)
        Attack         //発射
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;//もとにもどるため
    private Transform _player;
    private Rigidbody _rb;

    private float _jumpPower = 7f;

    private bool _isGrounded;
    private bool _moveStop = false;
    private int _direction = 0;

    private bool _iswait;//waitコルーチンの重複を防ぐ
    private bool _ismove;//moveコルーチンの重複を防ぐ
    private bool _isattack;//attackコルーチンの重複を防ぐ

    private float _fallTime;
    private float _fallSpeed;

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
        Vector3 velocity = _rb.velocity;
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 13f)
                {
                    Debug.Log("発見");
                    _state = EnemyState.Wait;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) > 17f)
                {
                    _state = EnemyState.Look;
                }
                Direction();
                if(!_ismove)//コルーチンの重複防ぐ
                    StartCoroutine(Move());
                break;

            case EnemyState.Wait:
                Direction();
                if(!_iswait)//コルーチンの重複防ぐ
                    StartCoroutine(Wait());//しばらく待ってMoveに
                break;

            case EnemyState.Attack:
                if(!_isattack)
                    StartCoroutine(Attack());
                _state = EnemyState.Wait;
                break;
        }

        if (!_isGrounded)
        {
            _fallTime += Time.deltaTime;

            _fallSpeed = Physics.gravity.y * _fallTime * 5f * 5f; //Unityの標準重力に任せたいなら fallSpeed は不要

            velocity.y += _fallSpeed * Time.fixedDeltaTime;
        }
        else
        {
            //velocity.x = 0f;
        }
            _rb.velocity = velocity;
    }

    private void Look()
    {
        _direction = 0;
    }

    private void Direction()
    {
        if(!_isGrounded)
        {
            return;
        }

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

    private IEnumerator Move()
    {
        _ismove = true;
        Vector3 velocity = _rb.velocity;
        int _rand = Random.Range(1, 4);
        if (Vector3.Distance(transform.position, _player.position) > 7f)
        {
            Debug.Log("backjump");
            _rb.AddForce(_direction * 13f, _jumpPower, 0f, ForceMode.Impulse);
            if (_isGrounded)
            {
                velocity.x = 0f;//着地で止まる（意味ないかも
                _state = EnemyState.Attack;
            }
            
        }
        else if(Vector3.Distance(transform.position, _player.position) <= 7f &&
            Vector3.Distance(transform.position, _player.position) > 3f)
        {
            _state = EnemyState.Attack;
        }
        else
        {
            Debug.Log("frontjump");
            _rb.AddForce(_direction * -9f, _jumpPower, 0f, ForceMode.Impulse);
            if (_isGrounded)
            {
                velocity.x = 0f;//着地で止まる（意味ないかも
                _state = EnemyState.Attack;
            }
        }

        _rb.velocity = velocity;
        _ismove = false;
        yield break;
    }

    private IEnumerator Wait()
    {
        _iswait = true;
        yield return new WaitForSeconds(2f);
        _state = EnemyState.Move;
        _iswait = false;
        yield break;
    }

    private IEnumerator Attack()
    {
        _isattack = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attack");
        _isattack = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _fallTime = 0f;
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            _fallTime = 0f;
            _isGrounded = false;
        }
    }
}
