using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DroneEnemy : MonoBehaviour
{
    enum EnemyState
    { 
        DITECTION,
        MOVE,
        ATTACK,
        STIFFNESS,
    };


    private Rigidbody _rb;
    private Transform _playerTransform;
    private EnemyState _state;
   [SerializeField] private float _distance;
    private Vector3 _velocity;
    private float _moveSpeed = 5f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _state = EnemyState.DITECTION;
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
                {
                    IsStiff = Stiffness();
                    if (IsStiff == false)
                    {
                        _state = EnemyState.DITECTION;
                        Debug.Log("false");
                    }
                    break;
                }
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
        Debug.Log("Attack");
        _state = EnemyState.STIFFNESS;
    }
    [SerializeField] float _testtime;

    private float stifftime;
    private bool Stiffness()
    {
        stifftime += Time.deltaTime;
        _testtime = stifftime;
        if (stifftime > 3f)
        {
            stifftime = 0;
            return false;
        }
        else
            return true;
    }
}