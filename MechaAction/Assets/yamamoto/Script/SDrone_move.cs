using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDrone_move : MonoBehaviour
{
    private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
