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
    private Rigidbody _rb;
    Vector3 _velocity;
    private Transform _player;
    [SerializeField]private Vector3 _PerentPos;
    private int _dir;
    private float _moveSpeed = 2f;
    private float _limittime;
    private int tmp;
    private float _time;
    private float _turnTime = 2f;      //硬直時間が確定したらconstに変更

    [SerializeField]private bool isbomb;

    private bool _isRight = false;
    private bool _isLeft = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        _limittime = 0.5f;
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

        //coolDown.DiraySkill(1f);
        if ((_time += Time.deltaTime) >= _turnTime)
        {
            if (!CanMove) return;
            Move();
        }
            
        //if (Vector3.Distance(_rb.position, _PerentPos) >= 10)
        //Boom();
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }
    private void Move()  
    {
 

       tmp = _dir;

       _dir =  (_rb.position.x < _player.position.x )? 1 : -1;

        if (tmp != _dir)
        {
            if (_dir == 1)
                _isRight = true;
            else
                _isLeft = true;

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
                Debug.Log("boom発動");
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
        _rb.velocity = _velocity;
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
        Debug.Log("BOOOOOOOM");
        Collider[] hitcolliders = Physics.OverlapSphere(_rb.position, 3f);

        foreach (var hit in hitcolliders)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("3ダメージ");
            }
        }

        SpiderMother.GenerateCount--;
        Destroy(gameObject, 0.5f);
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
    




