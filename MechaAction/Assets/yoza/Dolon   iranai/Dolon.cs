using System.Collections;
using UnityEngine;

public class Dolon : MonoBehaviour
{
    // =================================================================================
    // 1. ステートとタイマー (クラス変数)
    // =================================================================================

    public enum DroneState { Patrol, Engage, Lost }

    [Header("現在の状態")]
    [SerializeField] private DroneState currentState = DroneState.Patrol;

    private Transform playerTarget;
    private Coroutine stateCoroutine;
    private float attackTimer;

    // =================================================================================
    // 2. 設定 (Inspector) - 属性をコンパクトに記述
    // =================================================================================

    [Header("1. 索敵設定")]
    [SerializeField, Tooltip("プレイヤーを探す範囲（半径）を設定")]
    private float detectionRange = 15f;
    [SerializeField, Tooltip("索敵チェックの間隔 (秒)")]
    private float checkInterval = 0.5f;
    [SerializeField, Tooltip("見失ってから Patrol に戻るまでの時間 (秒)")]
    private float lostTime = 3f;

    [Header("2. 移動設定")]
    [SerializeField, Tooltip("プレイヤーと保ちたい最適な距離 (m)")]
    private float idealDistance = 10f;
    [SerializeField, Tooltip("idealDistance からの許容誤差 (m)。この範囲内ならドローンは移動を止める")]
    private float movementTolerance = 2f;
    [SerializeField, Tooltip("ドローンが前進・後退するときの速さ")]
    private float moveSpeed = 5f;

    // 旋回速度は不要になりました

    // 移動継続のためのマージン（Inspectorには表示しない）
    private const float AdvanceMargin = 0.3f;

    [Header("3. 攻撃設定")]
    [SerializeField, Tooltip("ビームを撃つ間隔 (秒)")]
    private float attackInterval = 2f;
    [SerializeField, Tooltip("発射するビーム (弾丸) のPrefabを設定")]
    private GameObject beamPrefab;
    [SerializeField, Tooltip("ビームが生成される位置 (子オブジェクトの Transform) を設定")]
    private Transform firePoint;


    // =================================================================================
    // 3. Unityライフサイクルとユーティリティ
    // =================================================================================

    void Start()
    {
        stateCoroutine = StartCoroutine(AIStateControlCoroutine());
    }

    void Update()
    {
        if (currentState == DroneState.Engage && playerTarget != null)
        {
            // ⭐ HandleRotation() を削除しました
            HandleMovement();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = (currentState == DroneState.Engage) ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    // =================================================================================
    // 4. ステート管理コルーチン
    // =================================================================================

    IEnumerator AIStateControlCoroutine()
    {
        while (true)
        {
            switch (currentState)
            {
                case DroneState.Patrol:
                    yield return HandlePatrol();
                    break;
                case DroneState.Engage:
                    yield return HandleEngage();
                    break;
                case DroneState.Lost:
                    yield return HandleLost();
                    break;
            }
            yield return null;
        }
    }

    IEnumerator HandlePatrol()
    {
        while (currentState == DroneState.Patrol)
        {
            CheckForPlayer();
            if (playerTarget != null)
            {
                currentState = DroneState.Engage;
                yield break;
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator HandleEngage()
    {
        attackTimer = attackInterval;

        while (currentState == DroneState.Engage)
        {
            // 攻撃ロジック（毎フレーム判定）
            if (CanAttack())
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    ShootBeam();
                    attackTimer = attackInterval;
                }
            }

            // 索敵チェック（見失っていないか）は一定間隔で行う
            yield return new WaitForSeconds(checkInterval);
            CheckForPlayer();

            if (playerTarget == null)
            {
                currentState = DroneState.Lost;
                yield break;
            }
        }
    }

    IEnumerator HandleLost()
    {
        float timer = 0f;
        while (timer < lostTime)
        {
            CheckForPlayer();
            if (playerTarget != null)
            {
                currentState = DroneState.Engage;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        currentState = DroneState.Patrol;
    }

    // =================================================================================
    // 5. 行動ロジック (移動・攻撃)
    // =================================================================================

    private void HandleMovement()
    {
        if (playerTarget == null) return;

        Vector3 dronePosition = transform.position;
        Vector3 targetPosition = playerTarget.position;

        // 2.5D化と水平移動の保証
        targetPosition.y = dronePosition.y;
        targetPosition.z = dronePosition.z;

        float distance = Vector3.Distance(dronePosition, targetPosition);
        Vector3 direction = (targetPosition - dronePosition).normalized;
        Vector3 moveVector = Vector3.zero;

        float minRange = idealDistance - movementTolerance;
        float maxRangeToStop = idealDistance + movementTolerance;
        float maxRangeToAdvance = maxRangeToStop + AdvanceMargin;

        if (distance < minRange)
        {
            moveVector = -direction; // 後退
        }
        else if (distance > maxRangeToAdvance)
        {
            moveVector = direction; // 前進
        }

        Vector3 deltaMovement = moveVector * moveSpeed * Time.deltaTime;

        // 2.5D化と水平移動の強制
        deltaMovement.y = 0f;
        deltaMovement.z = 0f;

        transform.position += deltaMovement;
    }

    // ⭐ HandleRotation() メソッドは削除されました

    private bool CanAttack()
    {
        if (playerTarget == null) return false;

        float distance = Vector3.Distance(transform.position, playerTarget.position);

        float maxAttackRange = idealDistance + movementTolerance;

        // 移動誤差の分、わずかに攻撃範囲を広げる
        if (distance <= maxAttackRange + 0.1f)
        {
            return true;
        }
        return false;
    }

    private void ShootBeam()
    {
        if (beamPrefab == null || firePoint == null)
        {
            Debug.LogError("ビームPrefabまたはFirePointが設定されていません。");
            return;
        }
        
        Instantiate(beamPrefab, firePoint.position, firePoint.rotation);
    }

    private void CheckForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        playerTarget = null;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                playerTarget = hitCollider.transform;
                return;
            }
        }
    }
}