using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using static Enemy;

public class NewBehaviourScript : MonoBehaviour
{
    const float ATTACKTIME = 5f;


    //private enum EnemyState
    //{ 
    //DITECTION,
    //LOOK,
    // //WAIT,
    //ATTACK
    //}

    [SerializeField] private GameObject energyBallPrefab;   
    private GameObject _myEnergyBall;   

    // private EnemyState _state = EnemyState.DITECTION;
    private Rigidbody _rd;
    private Transform _player;
    private bool _ATTACK;


    private float _attackTime;
    private void Awake()
    {
        _rd = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_player.position, _rd.position) < 20f)
        {
            Direction();
        }
        else
        {
            Look();
        }

#if false
        switch (_state)
        {
            case EnemyState.DITECTION:
               
                if (Vector3.Distance(_player.position, _rd.position) < 20f)
                {
                    _state = EnemyState.LOOK;
                } 
                Didection();
                break;

                case EnemyState.LOOK:
                Look();
                //StartCoroutine(Attack());
                _attackTime += Time.deltaTime;

                if(_attackTime >= ATTACKTIME)
                {
                    _attackTime = 0f;
                    _state = EnemyState.ATTACK;
                }

                if (Vector3.Distance(_player.position, _rd.position) >= 20f)
                {
                    _state = EnemyState.DITECTION;
                }
                break;

                //case EnemyState.WAIT:
                //Wait(); 
                //break;

                case EnemyState.ATTACK:
                Attack();
                break;
        }
#endif
    }

    private void Direction()
    {
        _attackTime = 0f;
    }
    private void Look()
    {
        _attackTime += Time.deltaTime;

        if (_attackTime >= ATTACKTIME)
        {
            _attackTime = 0f;
            Attack();
        }
    }
    //private IEnumerator Attack()
    //{
    //
    //    yield break;
    //}

    private void Attack()
    {
        //_myEnergyBall = Instantiate(energyBallPrefab,transform.position,transform.rotation);
        //
        
    }
   // private void Wait()
    //{

    //}


}
