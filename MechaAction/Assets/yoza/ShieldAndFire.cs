using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro.EditorUtilities;
using UnityEngine;

public class ShieldAndFire : MonoBehaviour
{  
    
    const float ATTACKTIME = 3f;
    const float SHIELDTIME = 3f;

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
    [SerializeField] Vector3 _velocity;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransfrom = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        state =EnemyState.DITECTION;
        _velocity = _rb.velocity;
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_playerTransfrom.position, _rb.position);
        switch(state)
        {
            case EnemyState.DITECTION:
                Ditection();
                break;

            case EnemyState.MOVE:
                
                break;
                
            case EnemyState.ATTACK:

                break;

            case EnemyState.SHIELD:

                break;

            case EnemyState.DESTROY:
                break;
        }
    }

    private void Ditection()
    {
        if (_distance < 20)
            //state =
            Move();
    }

    private void Move()
    {

       tmp = _dir;
   
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
       
       if(_distance<4f)
       {
           _velocity.x = 0;
        
       }
       
        _rb.velocity = _velocity;
 
    }

    private void Attack()
    {

       
    }
    private void Shield()
    {

    }

    private void Destroy()
    {

    }
}
