using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire_Drone : MonoBehaviour
{

    [Header("右への速度"), SerializeField] private float _horizontalSpeed = 1f;  // 右への移動速度

    private Rigidbody _rb;
    private Transform _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = new Vector2(_horizontalSpeed, 0f);
        _rb.velocity = newVelocity;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, _player.transform.position) < 25f)
        {
            Debug.Log("タイヤ落下");

        }
        /*float xDistance = Mathf.Abs(transform.position.x);
        if (xDistance <= 25f)
        {
            Debug.Log("タイヤ落下");
        }*/
    }
}
