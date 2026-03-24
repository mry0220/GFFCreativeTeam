using System.Collections;
using UnityEngine;
using Cooltime;


[RequireComponent(typeof(Rigidbody))]
public class SpiderChild : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    private CoolDown coolDown = new CoolDown();
    private Transform _player;
    private Rigidbody _rb;
    private SpiderChild_Attack _attack;
    //[SerializeField]private Vector3 _PerentPos;

    public int Dir => _dir;
    private int _dir;
    private float _moveSpeed = 2f;
    private float _limittime;
    private int tmp;
    private float _time;
    const float _turnTime = 2f;      //硬直時間が確定したらconstに変更

    Vector3 _velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<SpiderChild_Attack>();
    }

    private void Start()
    {
        _limittime = 0.5f;
        _time = 2f;
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
        origin = transform.position;
        _isGrounded = Physics.SphereCast(origin, 0.2f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));
        Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.2f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.2f);
    }

    private void FixedUpdate()
    {
        _velocity = _rb.velocity;

        if ((_time += Time.deltaTime) >= _turnTime)
        {
            if (!CanMove) return;
            Move();
        }

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0f;
        }
        _rb.velocity = _velocity;
        //if (Vector3.Distance(_rb.position, _PerentPos) >= 10)
        //Boom();
    }

    private void Move()  
    {
        tmp = _dir;

        _dir =  (_rb.position.x < _player.position.x )? 1 : -1;

        if (tmp != _dir)
        {
            _time = 0;
            _velocity.x = 0;
            _rb.velocity = _velocity;
            return;
        }
        else
        {
            _velocity.x = _dir * _moveSpeed;
        }

        if (Vector3.Distance(_player.position, _rb.position) < 5f)
        {
            if((_limittime -= Time.deltaTime) <0)
            {
                //Debug.Log("boom発動");
                Boom();
            }
        }
        else
        {
            _limittime = 0.5f;
        }

        if (Vector3.Distance(_player.position, _rb.position) < 3.5f)
        {
            _velocity.x = 0;
            _rb.velocity = _velocity;
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

    Coroutine _thiscoroutine;
    private void Boom()
    {
        if (_thiscoroutine == null)
        {
            _thiscoroutine =
            StartCoroutine(coolDown.Skill(callback => { _thiscoroutine = callback; }, 0,boom,null,3f));
        }
    }

    private void boom()
    {
        //Debug.Log("BOOOOOOOM");
        Collider[] hitcolliders = Physics.OverlapSphere(_rb.position, 3f);

        foreach (var hit in hitcolliders)
        {
            if (hit.CompareTag("Player"))
            {
                _attack.Boom();
            }
        }

        SpiderMother.GenerateCount--;
        Destroy(gameObject, 0.5f);
    }

    #region 被ダメ処理
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Move;
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





