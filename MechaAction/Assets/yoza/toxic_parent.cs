using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class toxic_parent : MonoBehaviour
{
    const float SYUSANTIME = 5f;
    const int GENERATEMAX = 3;

    private Rigidbody _rb;
    Vector3 _velocity;
    private bool _moveStop;
    private Transform _player;
    private int _dir;
    private float _moveSpeed = 5f;
    private float _time;
    [SerializeField] private float test;
    private int tmp;

    private float _syusantime;
    [SerializeField] private GameObject Bombspider;
    private GameObject _Bombspider;
    

   static public int count =0;

    //[SerializeField]List<GameObject> _Bombchildren = new List<GameObject>();



    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
       // for(int i =0;i < 3;i++)
     //   _Bombchildren.Add(Bombspider);

    }
    private void FixedUpdate()
   {
        if (Vector3.Distance(transform.position, _player.position) <= 10)
        {
            if ((_time += Time.deltaTime) >= 1f)
            {
                Move();
            }
        }
   }
    private void Move()
    {

        _velocity = _rb.velocity;

       
        _syusantime +=Time.deltaTime;

        tmp = _dir;

        _dir = (_rb.position.x > _player.position.x) ? 1 : -1;

        if (tmp != _dir)
        {
            _velocity.x = 0;
            _time = 0;
            return;
        }
        else
        {
            _velocity.x = _dir * _moveSpeed;
        }                  
        _rb.velocity = _velocity;

        test = _syusantime;
        if (_syusantime>=SYUSANTIME)
        {
            _syusantime = 0f;
            if (count < 3)
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
        _Bombspider = Instantiate(Bombspider, transform.position, transform.rotation);
        count++;
    }
   
}
