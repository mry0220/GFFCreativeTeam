using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemy : MonoBehaviour
{
    private GameObject _myEnergyBall;
    private Transform _playerTransform;
    private Rigidbody _rb;
    [SerializeField] private GameObject energyBallPrefab;

    const float ATTACKTIME = 5f;
    private float _attackTime;

    [SerializeField] EnemyAttackSO _enemyattackSO;
    private float _bantime;
    private string _effectname;
    private string _audioname;

    private int _clear;

    private void Awake()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform; ;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("StunEnemy");
        if (attackData != null)
        {
            _bantime = attackData.Bantime + _clear;
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(_playerTransform.position, _rb.position) < 20f)
        {
            Look();
        }
        else
        {
            _attackTime = 0f;
        }
    }

    private void Look()
    {
        _attackTime += Time.deltaTime;

        float Angle = GetAngle(transform.position,_playerTransform.position);
        Debug.Log(Angle);
        transform.rotation = Quaternion.Euler(0f, 0f, Angle);

        if (_attackTime >= ATTACKTIME)
        {
            _attackTime = 0f;
            Attack();
        }
    }

    private void Attack()
    {
        _myEnergyBall = Instantiate(energyBallPrefab,transform.position,transform.rotation);
        _myEnergyBall.GetComponent<StunEnergy>().Initialize(_bantime, _effectname, _audioname);
    }

    float GetAngle(Vector3 my,Vector3 target)
    {
        Vector3 dt = target - my;
        float rad =Mathf.Atan2(dt.y,dt.x);
        float degree= rad *Mathf.Rad2Deg;

        return degree;
    }

}
