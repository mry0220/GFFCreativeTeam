using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform m_target;       // プレイヤー
    private Vector2 minPos;         // Clamp最小値
    private Vector2 maxPos;         // Clamp最大値
    public float smoothSpeed = 5f; // カメラ追従の滑らかさ

    [SerializeField] private float shakeDuration = 0.25f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    //private Coroutine shakeCoroutine;

    [SerializeField] private LayerMask m_cameraLayer;

    private CameraArea m_currentArea;

    private void Awake()
    {
        m_target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (m_target == null) return;

        if (m_currentArea == null) return;

        var bounds = m_currentArea.Bounds;

        // Clampで制限しつつ追従
        float targetX = Mathf.Clamp(m_target.position.x, bounds.min.x, bounds.max.x);
        float targetY = Mathf.Clamp(m_target.position.y, bounds.min.y, bounds.max.y);
        Vector3 desiredPos = new Vector3(targetX, targetY, transform.position.z);

        // 滑らかに追従
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        CheckCameraArea();
    }

    private void CheckCameraArea()
    {
        Vector3 playerPos = m_target.position;

        Collider[] cols = Physics.OverlapSphere(playerPos, 1f, m_cameraLayer);

        CameraArea bestArea = null;

        foreach(var col in cols)
        {
            var area = col.GetComponent<CameraArea>();
            if (area == null) continue;

            if(bestArea == null || area.priority > bestArea.priority)
            {
                bestArea = area;
            }
        }

        if(bestArea != null)
        {
            SetArea(bestArea);
        }
    }

    public void SetArea(CameraArea area)
    {
        m_currentArea = area;
    }

    public void SetEnemyArea(CameraArea area)
    {
        m_currentArea = area;
    }

    // カメラ揺れ開始メソッド
    public void ShakeCamera()//float duration = -1f, float magnitude = -1f
    {
        //if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        //shakeCoroutine = StartCoroutine(Shake(
        //  duration > 0 ? duration : shakeDuration,
        //  magnitude > 0 ? magnitude : shakeMagnitude));

        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        //shakeCoroutine = null;
    }
}
