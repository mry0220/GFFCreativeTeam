using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class ShieldAndFire : MonoBehaviour
{

    const float ATTACKTIME = 3f;
    const float SHIELDTIME = 3f;
    const float GRAVITY = -9.81f;

    private enum EnemyState
    {
        DITECTION,
        MOVE,
        ATTACK,
        SHIELD,
        DESTROY,
    };

    private Rigidbody _rb;
    private Transform _playerTransfrom;
    private bool _ATTACK;
    private float _distance;
    private EnemyState state;
    private int _dir;
    private int tmp;
    private float _moveSpeed = 5f;
    private float _gravityTime;
    private bool _isGround;
    [SerializeField] Vector3 _velocity;
    private float _attackTime;
    private float _shieldTime;
    [SerializeField] private GameObject FlameThrower;
    [SerializeField] private GameObject _player;
    private GameObject _myFlameThrower;




    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransfrom = GameObject.FindWithTag("Player").transform;

    }

    private void Start()
    {
        state = EnemyState.DITECTION;
        _velocity = _rb.velocity;
        _gravityTime = 0;
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_playerTransfrom.position, _rb.position);

        if (!(_isGround))
        {
            _gravityTime += Time.deltaTime;
            _velocity.y = GRAVITY * _gravityTime;

        }
        else
        {
            _gravityTime = 0;
            _velocity.y = 0;

            switch (state)
            {
                case EnemyState.DITECTION:
                    Attack();
                   // Ditection();
                    break;

                case EnemyState.MOVE:

                    break;

                case EnemyState.ATTACK:
                 //   Attack();
                    state = EnemyState.SHIELD;
                    break;

                case EnemyState.SHIELD:
                    Shield();
                    break;

                case EnemyState.DESTROY:
                    break;
            }
        }
    }

    private void Ditection()
    {
        if (_distance < 20)
        {
            Move();
        }
        else
        {
            _attackTime = 0;
            _shieldTime = 0;
        }
    }

    private void Move()
    {

        tmp = _dir;

        _attackTime += Time.deltaTime;
        //_shieldTime += Time.deltaTime;

        _dir = (_rb.position.x < _playerTransfrom.position.x) ? 1 : -1;
        if (tmp != _dir)
        {
            _velocity.x = 0;
            return;
        }
        else
        {
            _velocity.x = _dir * _moveSpeed;
        }

        if (_distance < 4f)
        {
            _velocity.x = 0;

        }

        _rb.velocity = _velocity;

        if (_distance < 4f)
        {
           
            if (_attackTime >= ATTACKTIME)
            {
                _attackTime = 0f;
                state = EnemyState.ATTACK;
            }
           
        }

    }
    [SerializeField]private float testtime;
        float Attacktime = 0;
    private void Attack()
    {
        
        
        while(Attacktime < 3f)
        {
            Attacktime += Time.deltaTime;
            Debug.Log(Attacktime);
            Debug.Log("FlameThrower");
            testtime = Attacktime;
        }
        return;
        //_myFlameThrower = Instantiate(FlameThrower, transform.position, transform.rotation);
       // if (_shieldTime >= SHIELDTIME)
       // {
       //     _shieldTime = 0f;
       //     Shield();
       // }
    }


    private void Shield()
    {
        for(float shieldTime = 0;shieldTime <= 3f; shieldTime += Time.deltaTime)
            Debug.Log("SHIELD");
        return;
           
    }

    private void Destroy()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _isGround = true;
            Debug.Log("isGround");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Grounded"))
        {
            _isGround = false;
        }
    }

    
}
