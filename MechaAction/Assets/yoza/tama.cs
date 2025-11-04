using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))] 
public class tama : MonoBehaviour
{
    private Rigidbody _rb;
   [SerializeField] private float _speed=1f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
      _rb.velocity = transform.right * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.))
        {
            
        }
    }
}