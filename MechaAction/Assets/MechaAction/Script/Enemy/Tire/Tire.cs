using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tire : MonoBehaviour
{
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;
    private int _dir;

    public void _FallStart(int hitdamage, int hitknockback, int dir, string effectname, string audioname)
    {
        _hitdamage = hitdamage;
        _hitknockback = hitknockback;
        _effectname = effectname;
        _audioname = audioname;
        _dir = dir;

        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);//敵のインターフェース<IDamage>取得
            }
        }
    }
}
