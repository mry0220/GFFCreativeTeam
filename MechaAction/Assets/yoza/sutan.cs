using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using static Enemy;

public class NewBehaviourScript : MonoBehaviour
{
    const float ATTACKTIME = 5f;

    [SerializeField] private float test;
    //private enum EnemyState
    //{ 
    //DITECTION,
    //LOOK,
    // //WAIT,
    //ATTACK
    //}

    [SerializeField] private GameObject energyBallPrefab;
    [SerializeField] private GameObject _player;
    private GameObject _myEnergyBall;   

    // private EnemyState _state = EnemyState.DITECTION;
    private Rigidbody _rd;
    private Transform _playerTransform;
    private bool _ATTACK;
    private Vector3 distance;

    private float _attackTime;
    private void Awake()
    {
        _rd = GetComponent<Rigidbody>();
        _playerTransform = _player.transform;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_playerTransform.position, _rd.position) < 20f)
        {
            //Debug.Log("fuga");
            Debug.Log("hoge");
            Look();
        }
        else
        {
        _attackTime = 0f;
        }

#if false
        switch (_state)
        {
            case EnemyState.DITECTION:
               
                if (Vector3.Distance(_playerTransform.position, _rd.position) < 20f)
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

                if (Vector3.Distance(_playerTransform.position, _rd.position) >= 20f)
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
    //private void Direction()
    //
    //{
    //}
    private void Look()
    {
        _attackTime += Time.deltaTime;

        float Angle = GetAngle(transform.position,_playerTransform.position);
        Debug.Log(Angle);
        transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        test = _attackTime;
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
        _myEnergyBall = Instantiate(energyBallPrefab,transform.position,transform.rotation);
      
    }

    float GetAngle(Vector3 my,Vector3 target)
    {
        Vector3 dt = target - my;
        float rad =Mathf.Atan2(dt.y,dt.x);
        float degree= rad *Mathf.Rad2Deg;

        return degree;
    }
   // private void Wait()
    //{

    //}


}
