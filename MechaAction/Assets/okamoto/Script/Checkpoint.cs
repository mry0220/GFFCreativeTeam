using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        GManager.Instance.CheckPoint(transform.position);
    }
}
