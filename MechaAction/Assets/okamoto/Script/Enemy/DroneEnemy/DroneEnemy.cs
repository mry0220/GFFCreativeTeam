using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneEnemy : MonoBehaviour, IEnemy
{
    enum EnemyState
    { 
        DITECTION,
        MOVE,
        ATTACK,
        STIFFNESS,
        DAMAGE//ìÆÇ©Ç»Ç¢
    };
    private EnemyState _state = EnemyState.DITECTION;

    private Drone_Attack _attack;
    private Rigidbody _rb;
    private Transform _playerTransform;

    public int Dir => _dir;
    private int _dir;
   [SerializeField] private float _distance;
    private float stifftime;
    private float _moveSpeed = 5f;

    private Vector3 _velocity;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Drone_Attack>();
    }

    private void Update()
    {
        if (_rb.position.x < _playerTransform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//âE
            _dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//ç∂
            _dir = -1;
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        
        _distance = Vector3.Distance(_rb.position, _playerTransform.position);
        switch (_state)
        {
            case EnemyState.DITECTION:
                Ditection();
                break;

            case EnemyState.MOVE:
                Move();
                break;
                
            case EnemyState.ATTACK:
                Attack();
                break;

            case EnemyState.STIFFNESS:
                IsStiff = Stiffness();
                if (IsStiff == false)
                {
                    _state = EnemyState.DITECTION;
                    //Debug.Log("false");
                }
                break;
        }
    }

    private bool IsStiff = true;
    private void Ditection()
    {
        if(_distance <= 50f)
        {
            _state = EnemyState.MOVE;
        }
    }

    private void Move()
    {
        _velocity = _rb.velocity;
        if(_distance <= 10f)
        {
            
            _rb.velocity = Vector3.zero;
            _state = EnemyState.ATTACK;
            return;
        }
        _velocity.x = _moveSpeed;
        _rb.velocity = (_rb.position.x < _playerTransform.position.x) ? (_velocity) : (-_velocity); 
    }

    private void Attack()
    {
        StartCoroutine(_attack.Attack());
        _state = EnemyState.STIFFNESS;
    }

    private bool Stiffness()
    {
        stifftime += Time.deltaTime;
        if (stifftime > 3f)
        {
            stifftime = 0;
            return false;
        }
        else
            return true;
    }

    #region îÌÉ_ÉÅèàóù
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