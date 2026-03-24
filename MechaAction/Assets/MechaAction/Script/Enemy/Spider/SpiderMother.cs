using System.Collections;
using UnityEngine;
using Cooltime;

[RequireComponent(typeof(Rigidbody))]
public class SpiderMother : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    [SerializeField] private GameObject Bombspider;
    private Rigidbody _rb;
    private Transform _player;
    
    public int Dir => _dir;
    private int _dir;
    const float GENERATETIME = 5f;
    const int GENERATECOUNT = 3;
    static public int GenerateCount = 0;
    private float _moveSpeed = 2f;
    private float _time;
    private float _distance;
    const float _ditection = 13;       //索敵範囲が確定したらconstに変更
    const float _turnTime = 2f;      //硬直時間が確定したらconstに変更

    private float _generateTime;

    Vector3 _velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
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
        origin = transform.position + Vector3.down;
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));
        //Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        _velocity = _rb.velocity;
        _distance = Vector3.Distance(_rb.position, _player.position);
        
        if (_distance<= _ditection)
        {
            if ((_time += Time.deltaTime) >= _turnTime)
            {
                if (!CanMove) return;
                Move();
            }
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
    }

    private void Move()
    {
        _generateTime +=Time.deltaTime;

        int dirBefore = _dir;

        _dir = (_rb.position.x > _player.position.x) ? 1 : -1;

        if (dirBefore != _dir)
        {
            _velocity.x = 0;
            _time = 0;
            _rb.velocity = _velocity;
            return;
        }
        else
        {
            _velocity.x = _dir * _moveSpeed;
        }

        if (_generateTime >= GENERATETIME)
        {
            _generateTime = 0f;

            if (GenerateCount < GENERATECOUNT)
            {
                Generate();
                _time = 0;
                return;
            }
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

    private void Generate()
    {
        _rb.velocity = Vector3.zero;
        var _Bombspider = Instantiate(Bombspider, transform.position, transform.rotation);
        GenerateCount++;
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
