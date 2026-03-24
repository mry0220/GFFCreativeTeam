using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShieldAndFire : MonoBehaviour
{
    private enum EnemyState
    {
        DITECTION,
        MOVE,
        ATTACK,
        SHIELD,
        DAMAGE,
    };

    private EnemyState _state = EnemyState.DITECTION;

    private Transform _playerTransfrom;
    private Rigidbody _rb;
    private ShieldAndFire_Attack _attack;

    public int Dir => _dir;
    private int _dir;
    private float _distance;
    private float _moveSpeed = 5f;
    const float ATTACKTIME = 3f;
    const float SHIELDTIME = 3f;
    private float _ATKtime;
    private float _SHItime;

    Vector3 _velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;


    private void Awake()
    {
        _playerTransfrom = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<ShieldAndFire_Attack>();
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
        _velocity = _rb.velocity;
        _distance = Vector3.Distance(_playerTransfrom.position, _rb.position);

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0;

            switch (_state)
            {
                case EnemyState.DITECTION:
                    Ditection();
                    break;

                case EnemyState.MOVE:
                    Move();
                    break;

                case EnemyState.ATTACK:
                    _ATKtime += Time.deltaTime;
                    Attack();
                    if (_ATKtime >= ATTACKTIME)
                    {
                        _ATKtime = 0;
                        _state = _distance >= 4f ? EnemyState.MOVE : EnemyState.SHIELD;
                    }
                    break;

                case EnemyState.SHIELD:
                    _SHItime += Time.deltaTime;
                    Shield();
                    if(_SHItime >= SHIELDTIME)
                    {
                        _SHItime = 0;
                        _state = _distance >= 4f ? EnemyState.MOVE : EnemyState.ATTACK;
                    }
                    break;
            }
        }

        _rb.velocity = _velocity;
    }

    private void Ditection()
    {
        if (_distance < 20)
        {
            _state = EnemyState.MOVE;
            return;
        }
    }

    private void Move()
    {
        _dir = (_rb.position.x < _playerTransfrom.position.x) ? 1 : -1;
        
        _velocity.x = _dir * _moveSpeed;

        if (_distance < 4f)
        {
            _velocity.x = 0;
            _rb.velocity = _velocity;   
            _state = EnemyState.ATTACK;
        }
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unityの標準重力に任せたいなら fallSpeed は不要

        _velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y速度に徐々に加算
                                                        //Time.fixedDeltaTime 物理演算をフレームレートに依存させないため必須
        if (_velocity.y < -20f)//落下速度の制限
        {
            _velocity.y = -20f;
        }
    }

    private void Attack()
    {
        if (_ATKtime <= ATTACKTIME)
            _attack.Fire();
        else
            return;
    }

    private void Shield()
    {
        if (_SHItime <= SHIELDTIME)
            Debug.Log("SHIELD");
        else
            return;
    }

    #region 被ダメ処理
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.DITECTION;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(1.0f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.DAMAGE;
        StartCoroutine(_ReturnNormal(electtime));
    }
    #endregion


}
