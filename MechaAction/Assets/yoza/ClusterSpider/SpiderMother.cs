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

    const float GENERATETIME = 5f;
    const int GENERATECOUNT = 3;

    CoolDown _coolDown = new CoolDown();
    
    
    static public int GenerateCount = 0;

    [SerializeField] private GameObject Bombspider;
    
    private Rigidbody _rb;
    private Transform _player;
    private CapsuleCollider _col;
    Vector3 _velocity;

    private int _dir;

    private bool _isRight = false;
    private bool _isLeft = true;

    private float _moveSpeed = 2f;
    private float _time;
    private float _distance;
    [SerializeField]private float _ditection;       //索敵範囲が確定したらconstに変更
    private float _turnTime = 2f;      //硬直時間が確定したらconstに変更

    private float _generateTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
      //  _coolDown.DiraySkill(5f);
    }

    private void Start()
    {
        _time = 2f;
    }

    private void FixedUpdate()
    {
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

        _distance = Vector3.Distance(_rb.position, _player.position);
        
        if (_distance<= _ditection)
        {
            if ((_time += Time.deltaTime) >= _turnTime)
            {
                if (!CanMove) return;
                Move();
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }
    private void Move()
    {

        _velocity = _rb.velocity;
        _generateTime +=Time.deltaTime;

        int dirBefore = _dir;

        _dir = (_rb.position.x > _player.position.x) ? 1 : -1;

        if (dirBefore != _dir)
        {
            if(_dir == 1)
                _isRight = true;
            else
                _isLeft = true;

            _velocity.x = 0;
            _time = 0;
            _rb.velocity = _velocity;
            return;
        }

        else
        {
            _velocity.x = _dir * _moveSpeed;
        }
        
        _rb.velocity = _velocity;

        if (_generateTime>=GENERATETIME)
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
    private void Generate()
    {
        _rb.velocity = Vector3.zero;
        var _Bombspider = Instantiate(Bombspider, transform.position, transform.rotation);
        GenerateCount++;
    }

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
}
