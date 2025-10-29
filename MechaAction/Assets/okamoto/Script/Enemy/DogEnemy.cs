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
    private Dog_Attack _attack;

    private float _jumpPower = 7f;

    private bool _isGrounded;
    private bool _moveStop = false;
    private int _direction = 0;

    private bool _iswait;//waitコルーチンの重複を防ぐ
    private bool _ismove;//moveコルーチンの重複を防ぐ
    private bool _isattack;//attackコルーチンの重複を防ぐ

    private float _fallTime;
    private float _fallSpeed;

    private bool _isRight = false;
    private bool _isLeft = false;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Dog_Attack>();
    }

    private void Start()
    {
        _spawnPos = transform.position;
    }

    private void FixedUpdate()
    {
        if(_isRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _isRight = false;
        }
        else if(_isLeft)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            _isLeft = false;
        }
        

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
                if(!_isattack)//コルーチンの重複防ぐ
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

        if (_moveStop)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_direction == -1 || _direction == 0)
            {
                _isRight = true;
                Debug.Log("right");
                StartCoroutine(Waitturn(1));
            }
            _direction = 1;
        }
        else
        {
            if (_direction == 1 || _direction == 0)
            {
                _isLeft = true;
                Debug.Log("left");
                StartCoroutine(Waitturn(-1));
            }
            
            _direction = -1;
        }

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
        //int _rand = Random.Range(1, 4);
        if (Vector3.Distance(transform.position, _player.position) > 7f)
        {
            Debug.Log("frontjump");
            _rb.AddForce(_direction * 13f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.4f);

            
            Debug.Log("ぴた");
            yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                                       //上書きされnew Vector3が使えなため(AI参照)
            _rb.velocity = Vector3.zero;
            _state = EnemyState.Attack;
            
            
        }
        else if(Vector3.Distance(transform.position, _player.position) <= 7f &&
            Vector3.Distance(transform.position, _player.position) > 3f)
        {
            _state = EnemyState.Attack;
        }
        else
        {
            Debug.Log("backjump");
            _rb.AddForce(_direction * -9f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.4f);

            Debug.Log("ぴた");
            yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                                       //上書きされnew Vector3が使えなため(AI参照)
            _rb.velocity = Vector3.zero;
            _state = EnemyState.Attack;
        }

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
        _attack.GunAttack();
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
