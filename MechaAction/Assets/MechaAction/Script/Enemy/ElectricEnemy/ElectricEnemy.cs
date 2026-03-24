using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemy : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Look,
        Move,
        Attack,
        GroundAttack,
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private Rigidbody _rb;
    private ElectricEnemyAttack _attack;

    public int Dir => _dir;
    private int _dir = -1;
    private bool _moveStop = false;
    private bool _isgroundatack = false;
    private bool _isattack = false;
    private float _lapseTime;

    Vector3 velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<ElectricEnemyAttack>();
    }

    private void Start()
    {
        _lapseTime = 0.0f;
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
        Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.4f);
    }

    private void FixedUpdate()
    {
        _lapseTime += Time.deltaTime;
        velocity = _rb.velocity;

        switch ( _state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 15f)
                {
                    //Debug.Log("発見");
                    _state = EnemyState.Move;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) >20f)
                {
                    _state = EnemyState.Look;
                    break;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 4f)
                {
                    //Debug.Log("攻撃");
                    _state = EnemyState.Attack;
                    break;
                }
                else if (Vector3.Distance(transform.position, _player.position) < 7f
                    && _lapseTime >= 3f)
                {
                    //Debug.Log("地面攻撃");
                    _state = EnemyState.GroundAttack;
                    break;
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

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0f;
        }
        _rb.velocity = velocity;
    }

    private void Look()
    {
        //anim
    }


    private void Move()
    {
        

        if (_moveStop)
        {
            velocity.x = 0f;
            _rb.velocity = velocity;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_dir == -1 || _dir == 0)
            {
                StartCoroutine(Waitturn(1));
            }
            _dir = 1;
        }
        else
        {
            if (_dir == 1 || _dir == 0)
            {
                StartCoroutine(Waitturn(-1));
            }
            _dir = -1;
        }
        velocity.x = _dir * 3f;

    }

    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);
        _dir = _newdirection;
        _moveStop = false;
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

    private IEnumerator Attack()
    {
        _isattack = true;
        _rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        _attack.ElectricAttack();
        _lapseTime = 0;

        yield return new WaitForSeconds(1f);
        _state = EnemyState.Move;
        _isattack = false;
        yield break;
    }

    private IEnumerator GroundAttack()
    {
        _isgroundatack = true;
        _rb.velocity = Vector3.zero;
        _attack.ElectricGroundAttack();
        yield return new WaitForSeconds(2f);
        //インスタンス化
        
        _lapseTime = 0;

        _state = EnemyState.Move;
        _isgroundatack = false;
        yield break;
    }

    #region 被ダメ処理
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Look;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(2f));
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
