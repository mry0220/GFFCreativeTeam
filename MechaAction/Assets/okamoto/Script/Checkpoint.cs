using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GManager.Instance.CheckPoint(transform.position);
    }
}
