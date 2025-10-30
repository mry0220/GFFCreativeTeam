using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Slash : MonoBehaviour
{
    private Rigidbody _rb;
    private Player _player;

    [SerializeField] private float _speed;
    private int _dir;

    [SerializeField] private int _damage;
    [SerializeField] private int _knockback;

    Vector3 velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        _dir = _player._lookDir;
        StartCoroutine(_Destroy());
    }

    private void FixedUpdate()
    {
        velocity = _rb.velocity;
        velocity.x = _dir * _speed;
        _rb.velocity = velocity;
    }

    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("当たった");

        if (other.CompareTag("Enemy"))
        {
            //null条件

            other.GetComponent<IDamage>().TakeDamage(_damage, _knockback,_dir);//敵のインターフェース<IDamage>取得
        }
    }

}
