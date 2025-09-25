using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Look, Chase, Return, Attack }

    [SerializeField] private float _lookRange = 3f;    // パトロール範囲
    [SerializeField] private float _chaseRange = 5f;   // 探知範囲
    [SerializeField] private float _attackRange = 1f;  // 攻撃範囲
    [SerializeField] private float _moveSpeed = 4.0f;    // 移動速度
    [SerializeField] private float _chaseSpeed = 6.0f;    // Chase速度
    [SerializeField] private float _waitTime = 1.0f;   // 端で待つ時間

    private EnemyState _state = EnemyState.Look;

    private Vector3 _spawnPos;
    private Transform _player;
    private Rigidbody _rb;

    private int _lookDirection = 1; // 1 = 右, -1 = 左
    private bool _isWaiting = false;

    private void Awake()
    {
        _spawnPos = transform.position;
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(_rb.position, _player.position) < _chaseRange)
                    _state = EnemyState.Chase;
                break;

            case EnemyState.Chase:
                Chase();
                if (Vector3.Distance(_rb.position, _player.position) < _attackRange)
                    _state = EnemyState.Attack;
                else if (Vector3.Distance(_rb.position, _player.position) > _chaseRange)
                    _state = EnemyState.Return;
                break;

            case EnemyState.Return:
                ReturnToSpawn();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void Look()
    {
        if (_isWaiting)
        {
            _rb.velocity = Vector3.zero; // 待機中は停止
            return;
        }

        // 現在の速度をコピー
        Vector3 velocity = _rb.velocity;
        velocity.x = _lookDirection * _moveSpeed;

        _rb.velocity = velocity;// Rigidbody に反映

        // スポーン位置からの距離を確認
        float distanceFromSpawn = _rb.position.x - _spawnPos.x;

        if (distanceFromSpawn > _lookRange)
        {
            StartCoroutine(WaitAndTurn(-1));
        }
        else if (distanceFromSpawn < -_lookRange)
        {
            StartCoroutine(WaitAndTurn(1));
        }
    }

    private IEnumerator WaitAndTurn(int newDirection)
    {
        _isWaiting = true;
        //_rb.velocity = Vector3.zero; // 停止
        yield return new WaitForSeconds(_waitTime); // 少し待つ
        _lookDirection = newDirection; // 方向転換

        // 少しだけ端から押し戻す（次フレームで端扱いされないように）
        _rb.position += new Vector3(_lookDirection * 0.1f, 0f, 0f);
        _isWaiting = false;
    }

    private void Chase()
    {
        //一度変数にコピーしてから編集
        Vector3 velocity = _rb.velocity;

        // X軸だけプレイヤー方向に動かす
        float _directionX = Mathf.Sign(_player.position.x - _rb.position.x); // 右か左か
        velocity.x = _directionX * _chaseSpeed;


        _rb.velocity = velocity;//編集した値を戻してrigidbodyで実行

    }

    private void ReturnToSpawn()
    {
        Vector3 toSpawn = (_spawnPos - _rb.position);
        if (toSpawn.magnitude < 0.1f)
        {
            // スポーンに戻ったら巡回再開
            _rb.velocity = Vector3.zero;
            _state = EnemyState.Look;
            return;
        }

        Vector3 velocity = toSpawn.normalized * _moveSpeed;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, 0f);
    }

    private void Attack()
    {
        _rb.velocity = Vector3.zero;
        Debug.Log("攻撃！");
        //攻撃アニメーションや当たり判定をここで実装
        _state = EnemyState.Chase;
    }
}