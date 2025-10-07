using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPlayer_HP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   // ----- 3D Trigger (必要ならコメント切替) -----
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            GManager.Instance.OnPlayerHit();
        }
    }
}
