using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))] 
public class StunEnergy : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
      _rb.velocity = transform.right * _speed;
    }


}