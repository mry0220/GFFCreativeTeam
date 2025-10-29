using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog_Attack : MonoBehaviour
{
    private int damage = 20;
    private void Update()
    {
        //transform.position += transform.forward * 3f * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    public void GunAttack()
    {
        

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            GameObject target = hit.collider.gameObject;

            if(target.tag == "Player")
            {
                Debug.Log("playerにアタック");
            }
        }
    }
    
}
