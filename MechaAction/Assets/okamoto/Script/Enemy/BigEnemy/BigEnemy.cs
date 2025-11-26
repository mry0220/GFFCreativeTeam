using System.Collections;
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
        Attack,        //近接攻撃
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private Rigidbody _rb;
    private Big_Attack _attack;


    public int Dir => _dir;
    private int _dir;
    private float _jumpPower = 13f;
    private float _bigjumpPower = 18f;
    private float _moveSpeed = 5f;
    private bool _moveStop = false;
    private bool _iswait = false;//waitコルーチンの重複を防ぐ
    private bool _ismove = false;//moveコルーチンの重複を防ぐ
    private bool _ischarge = false;//chargeコルーチンの重複を防ぐ
    private bool _isjump = false;//jumpコルーチンの重複を防ぐ
    private bool _isbigjump = false;//bigjumpコルーチンの重複を防ぐ
    private bool _isattack = false;//attackコルーチンの重複を防ぐ

    Vector3 velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Big_Attack>();
    }

    private void Start()
    {
        
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
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 13f)
                {
                    Debug.Log("発見");
                    _state = EnemyState.Wait;
                }
                break;

            case EnemyState.Move:
               
                Direction();
                velocity.x = _dir * _moveSpeed;

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
            _Gravity();
        }
        else
        {
            _fallTime = 0;
        }
        _rb.velocity = velocity;
    }

    private void Look()
    {
        
    }

    private void Direction()
    {
        if (!_isGrounded)
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
            if (_dir == -1)
            {
                StartCoroutine(Waitturn(1));
            }
            _dir = 1;
        }
        else
        {
            if (_dir == 1)
            {
                StartCoroutine(Waitturn(-1));
            }
            _dir = -1;
        }
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


    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _dir = _newdirection;
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

        _rb.AddForce(_dir * 20f, 0f, 0f, ForceMode.Impulse);
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
            _rb.AddForce(_dir * -7f, _jumpPower, 0f, ForceMode.Impulse);
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
            _rb.AddForce(_dir * 9f, _jumpPower, 0f, ForceMode.Impulse);
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
        
        _rb.AddForce(_dir * 20f, _bigjumpPower, 0f, ForceMode.Impulse);

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

        _attack.Attack();
        _state = EnemyState.Wait;
        _isattack = false;
        yield break;
    }

    private IEnumerator JumpAttack()
    {
        yield return new WaitForFixedUpdate(); //コルーチン内だとFixedUpdate(Update?)で
                                               //上書きされnew Vector3が使えなため(AI参照)
        _rb.velocity = new Vector3(0f, -50f, 0f);
        _attack.JumpAttack();
        _state = EnemyState.Wait;

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
