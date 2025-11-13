using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire_Drone : MonoBehaviour
{

    [SerializeField] private Transform _tireObj;
    private Tire _tire;
    private Rigidbody _childrb;
    private Rigidbody _rb;
    private Transform _player;

    private float _moveSpeed = 10f;  // 右への移動速度
    public int dir;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _childrb = _tireObj.GetComponent<Rigidbody>();
        _tire = _tireObj.GetComponent<Tire>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Start()
    {
        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//右
            dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//左
            dir = -1;
        }
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;

        velocity.x = _moveSpeed * dir;

        _rb.velocity = velocity;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < 7f)
        {
            _tire._FallStart(dir);
            _childrb.isKinematic = false;
            _childrb.AddForce(transform.forward * 0.1f,ForceMode.Impulse);

        }
        /*float xDistance = Mathf.Abs(transform.position.x);
        if (xDistance <= 25f)
        {
            Debug.Log("タイヤ落下");
        }*/
    }
}
