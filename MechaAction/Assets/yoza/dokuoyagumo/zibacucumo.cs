using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Rigidbody))]
public class zibacucumo : MonoBehaviour
{
    private Rigidbody _rb;
    Vector3 _velocity;
    private bool _moveStop;
    private Transform _player;
    [SerializeField]private Vector3 _PerentPos;
    private int _dir;
    private float _moveSpeed = 5f;
    private float _time;
    private float _limittime;
     float test;
    private int tmp;
    private bool bomb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        _limittime = 0.5f;
    }

    private void FixedUpdate()
    {
        if ((_time += Time.deltaTime) >= 1f)
        {
            Move();
            //_time = 0;
        }
        if (Vector3.Distance(_rb.position, _PerentPos) >= 10)
            Boom();
        test = _time;
    }
    private void Move()  
    {
 

       tmp = _dir;

       // Debug.Log(Vector3.Distance(_player.position, _rb.position));
       _dir =  (_rb.position.x < _player.position.x )? 1 : -1;
       if(tmp != _dir)
        {
            _velocity.x = 0;
            _time = 0;
            return;
        }
        else
            _velocity.x = _dir * _moveSpeed;

        
        if (Vector3.Distance(_player.position, _rb.position) < 3f)
        {
            if((_limittime -= Time.deltaTime) <0)
            {
                Boom();
            }
        }
        else
        {
            _limittime = 0.5f;
        }
        //if (tmp != _dir)
        //{
        //    _rb.velocity = Vector3.zero;
        //    return;
        //}
        //int direction = (_rb.position.x < _player.position.x) ? 1 : -1;
        //Vector3 direction = (_player.position - _rb.position).normalized;

        if (Vector3.Distance(_player.position, _rb.position) < 1.15f)
        {
            _velocity = Vector3.zero;
        }
        _rb.velocity = _velocity;
        //Debug.Log(_attacktime);
    }

    //new method
    //«
    //name == boom;

    //limittime =0.5;
    //limittime < 0;
    //«
    //call boom;
    //coroutine
    //waitfortime =3f 3second;
    //attack(boom)
    private void Boom()
    {

        if (bomb == false)
            StartCoroutine(BoomCountDown());
    }

    private IEnumerator BoomCountDown()
    {
        bomb = true;
        yield return new WaitForSeconds(3f);
        Debug.Log("BOOOOOOOM");
        Collider[] hitcolliders = Physics.OverlapSphere(_rb.position, 3f);
        foreach (var hit in hitcolliders)
        {
            if(hit.CompareTag("Player"))
            {
                Debug.Log("3ƒ_ƒ[ƒW");
                
            }
        }
        toxic_parent.count--;
        Destroy(gameObject,0.5f);
        bomb = false;
    }


#if false
    //U‚è•Ô‚é‚Æ‚«­‚µ—¯‚Ü‚é
    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _dir = _newdirection;
        _moveStop = false;
        
        yield break;
    }

#endif
}
