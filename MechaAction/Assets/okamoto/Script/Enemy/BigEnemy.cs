using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro.EditorUtilities;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,          //探す
        Move,          //動く
        Charge,        //突進
        Jump,　　　　　//ジャンプ
        BigJump,       //大ジャンプ
        Wait,          //乱数
        Attack         //近接攻撃
    }

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;//もとにもどるため
    private Transform _player;
    private Rigidbody _rb;

    private float _jumpPower = 13f;
    private float _bigjumpPower = 18f;
    private float _moveSpeed = 5f;

    private bool _isGrounded;
    private bool _moveStop = false;
    private int _direction = 0;

    private bool _iswait = false;//waitコルーチンの重複を防ぐ
    private bool _ismove = false;//moveコルーチンの重複を防ぐ
    private bool _ischarge = false;//chargeコルーチンの重複を防ぐ
    private bool _isjump = false;//jumpコルーチンの重複を防ぐ
    private bool _isbigjump = false;//bigjumpコルーチンの重複を防ぐ
    private bool _isattack = false;//attackコルーチンの重複を防ぐ

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
                
                //if (Vector3.Distance(transform.position, _player.position) > 17f)
               // {
               //     _state = EnemyState.Look;
               // }
                Direction();
                velocity.x = _direction * _moveSpeed;

                if (!_ismove)//コルーチンの重複防ぐ
                    StartCoroutine(MoveTimelimit());
                break;

            case EnemyState.Charge:
                if (!_ischarge)//コルーチンの重複防ぐ
                    StartCoroutine(Charge());
                break;

            case EnemyState.Jump:
                if(!_isjump)
                StartCoroutine(Jump());
                break;

            case EnemyState.BigJump:
                if (!_isbigjump)//コルーチンの重複防ぐ
                    StartCoroutine(BigJump());
                break;

            case EnemyState.Wait:
                Direction();
                if (!_iswait)//コルーチンの重複防ぐ
                    StartCoroutine(Wait());//しばらく待ってMoveに
                break;

            case EnemyState.Attack:
                if (!_isattack)//コルーチンの重複防ぐ
                    StartCoroutine(Attack());
                break;
        }

        if (!_isGrounded)
        {
            _fallTime += Time.deltaTime;

            _fallSpeed = Physics.gravity.y * _fallTime * 3f * 3f; //Unityの標準重力に任せたいなら fallSpeed は不要

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
        if (!_isGrounded)
        {
            return;
        }

        if (_moveStop)
        {
            _direction = 0;
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
    }


    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _direction = _newdirection;
        _moveStop = false;

        yield break;
    }

    private IEnumerator MoveTimelimit()
    {
        _ismove = true;
        Debug.Log("Move");
        yield return new WaitForSeconds(3f);

        _state = EnemyState.Wait;
        _ismove = false;
        yield break;
    }

    private IEnumerator Charge()
    {
        _ischarge = true;
        Debug.Log("Charge準備");
        yield return new WaitForSeconds(0.2f);

        _rb.AddForce(_direction * 20f, 0f, 0f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.8f);
        _ischarge = false;
        _state = EnemyState.Wait;
        yield break;
    }

    private IEnumerator Jump()
    {
        _isjump = true;
        int _rand = Random.Range(1, 4);//1以上4未満
        if (Vector3.Distance(transform.position, _player.position) < 5f)
        {
            Debug.Log("backjump");
            _rb.AddForce(_direction * -7f, _jumpPower, 0f, ForceMode.Impulse);
            yield return new WaitForSeconds(0.8f);

            yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                                   //上書きされnew Vector3が使えなため(AI参照)
            _rb.velocity = Vector3.zero;

            _state = EnemyState.Wait;
            _isjump = false;
            yield break;
        }
        else
        {
            Debug.Log("frontjump");
            _rb.AddForce(_direction * 9f, _jumpPower, 0f, ForceMode.Impulse);
            yield return new WaitForSeconds(0.4f);
            if (_rand == 1)
            {
                StartCoroutine(JumpAttack());
                _isjump = false;
                yield break;
            }
            yield return new WaitForSeconds(0.4f);

            yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                                   //上書きされnew Vector3が使えなため(AI参照)
            _rb.velocity = Vector3.zero;

            _state = EnemyState.Attack;
            _isjump = false;
            yield break;
        }
        
    }

    private IEnumerator BigJump()
    {
        _isbigjump = true;
        Debug.Log("bigjump");
        //int _rand = Random.Range(1, 4);//1以上4未満
        
        _rb.AddForce(_direction * 20f, _bigjumpPower, 0f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.8f);

        yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                               //上書きされnew Vector3が使えなため(AI参照)
        _rb.velocity = Vector3.zero;

        _state = EnemyState.Wait;

        _isbigjump = false;
        yield break;
    }

    private IEnumerator Wait()
    {
        _iswait = true;
        Debug.Log("wait");
        yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                               //上書きされnew Vector3が使えなため(AI参照)
        _rb.velocity = Vector3.zero; //moveの後などピタっととめたい

        yield return new WaitForSeconds(2f);

        int _rand = Random.Range(0, 10);//0以上10未満
        
        if(Vector3.Distance(transform.position, _player.position) > 10f)
        {
            if( _rand >= 0 && _rand <5)
            {
                _state = EnemyState.BigJump;
            }
            else if(_rand >= 5 && _rand < 8)
            {
                _state = EnemyState.Jump;
            }
            else
            {
                _state = EnemyState.Move;
            }
        }
        else if (Vector3.Distance(transform.position, _player.position) <= 10f &&
            Vector3.Distance(transform.position, _player.position) > 4f)
        {
            if (_rand >= 0 && _rand < 5)
            {
                _state = EnemyState.Jump;
            }
            else if (_rand >= 5 && _rand < 8)
            {
                _state = EnemyState.Move;
            }
            else
            {
                _state = EnemyState.Charge;
            }
        }
        else
        {
            if (_rand >= 0 && _rand < 5)
            {
                _state = EnemyState.Charge;
            }
            else
            {
                _state = EnemyState.Attack;
            }
        }
        _iswait = false;
        yield break;
    }

    private IEnumerator Attack()
    {
        _isattack = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attack");
        _state = EnemyState.Wait;
        _isattack = false;
        yield break;
    }

    private IEnumerator JumpAttack()
    {
        yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                               //上書きされnew Vector3が使えなため(AI参照)
        _rb.velocity = new Vector3(0f, -50f, 0f);
        Debug.Log("JumpAttack");
        _state = EnemyState.Wait;

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
