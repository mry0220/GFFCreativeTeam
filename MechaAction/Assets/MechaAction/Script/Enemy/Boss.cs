using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,          //追跡
        Wait,          //発射用意(いらない)
        Shock,         //発射
        Energy,
        Rocket,
        Fire,
        Jump,
        Damage
    }

    private EnemyState _state = EnemyState.Shock;

    private Transform _player;
    private Rigidbody _rb;
    private Dog_Attack _attack;

    public int Dir => _dir;
    private int _dir = -1;
    private float _jumpPower = 7f;
    private bool _isshock;
    private bool _iswait;//waitコルーチンの重複を防ぐ
    private bool _ismove;//moveコルーチンの重複を防ぐ
    private bool _isjump;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    Vector3 velocity;
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Dog_Attack>();
    }

    private void Update()
    {
        if (_dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (_dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }

        RaycastHit hit;
        origin = transform.position + Vector3.down;
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));
        //Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.4f);
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;

        switch (_state)
        {
            case EnemyState.Shock:
                if (!_isshock)//コルーチンの重複防ぐ
                    StartCoroutine(Shock());
                break;

            case EnemyState.Move:
                if (!_ismove)//コルーチンの重複防ぐ
                    StartCoroutine(Move());
                break;

            case EnemyState.Wait:
                Direction();
                if (!_iswait)//コルーチンの重複防ぐ
                    StartCoroutine(Wait());//しばらく待ってMoveに
                break;

            case EnemyState.Energy:
                //anim
                //attack
                _state = EnemyState.Jump;
                break;

            case EnemyState.Rocket:
                //anim
                //attack
                _state = EnemyState.Jump;
                break;

            case EnemyState.Fire:
                //anim
                //attack
                _state = EnemyState.Jump;
                break;

            case EnemyState.Jump:
                Direction();
                if (!_isjump)//コルーチンの重複防ぐ
                    StartCoroutine(Jump());
                break;
        }

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0;
        }
        _rb.velocity = velocity;
    }

    private void Direction()
    {
        if (!_isGrounded)
        {
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            _dir = 1;
        }
        else
        {
            _dir = -1;
        }

    }

    private IEnumerator Shock()
    {
        _isshock = true;
        int _rand = Random.Range(0, 10);//0以上10未満

        if (Vector3.Distance(transform.position, _player.position) < 5f)
        {
            Direction();

            if ( _rand <5 )
            {
                //anim
                //Attack
                yield return new WaitForSeconds(2f);
            }
        }
        _state = EnemyState.Wait;

        _isshock = false;
        yield break;
    }

    private IEnumerator Move()
    {
        _ismove = true;
        int _rand = Random.Range(0, 10);//0以上10未満

        if(_rand < 2)
        {
            _state = EnemyState.Jump;
        }
        else if(_rand >= 2 && _rand < 4)
        {
            _state = EnemyState.Energy;
        }
        else if (_rand >= 4 && _rand < 7)
        {
            _state = EnemyState.Rocket;
        }
        else
        {
            _state = EnemyState.Fire;
        }

        _ismove = false;
        yield break;
    }

    

    private IEnumerator Wait()
    {
        _iswait = true;
        float count = 0f;
        int _rand = Random.Range(0, 10);//0以上10未満
        if( _rand < 4)
        {
            count = 0.5f;
        }
        else if( _rand >= 4 && _rand < 7)
        {
            count = 1f;
        }
        else
        {
            count = 2f;
        }

        yield return new WaitForSeconds(count);
        _state = EnemyState.Move;
        _iswait = false;
        yield break;
    }

    private IEnumerator Jump()
    {
        _isjump = true;
        int _rand = Random.Range(0, 10);//0以上10未満

        if( _rand < 6)
        {
            _rb.AddForce(_dir * 13f, _jumpPower, 0f, ForceMode.Impulse);

            int _rand2 = Random.Range(0, 10);
            if( _rand2 < 5)
            {
                //attack
                Debug.Log("空中攻撃");
            }

            yield return new WaitForSeconds(0.5f);
            yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                                   //上書きされnew Vector3が使えなため(AI参照)
            _rb.velocity = Vector3.zero;
        }
        _state = EnemyState.Shock;
        _isjump = false;
        yield break;
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unityの標準重力に任せたいなら fallSpeed は不要

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y速度に徐々に加算
                                                        //Time.fixedDeltaTime 物理演算をフレームレートに依存させないため必須
        if (velocity.y < -20f)//落下速度の制限
        {
            velocity.y = -20f;
        }
    }

    #region 被ダメ処理
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Shock;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1.0f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
    #endregion

}
