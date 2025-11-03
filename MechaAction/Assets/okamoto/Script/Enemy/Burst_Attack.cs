using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Attack : MonoBehaviour
{

    [SerializeField] GameObject _bulletPrefab;
    public Transform _bulletPosition;

    private void Awake()
    {
        
    }

    public void GunAttack()
    {
        StartCoroutine(Gun());
    }

    private IEnumerator Gun()
    {
        for(int i = 0;i < 3; i++)
        {
            Instantiate(_bulletPrefab, _bulletPosition.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }
}
