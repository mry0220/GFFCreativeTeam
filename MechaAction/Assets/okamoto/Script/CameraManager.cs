using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;       // プレイヤー
    public Vector2 minPos;         // Clamp最小値
    public Vector2 maxPos;         // Clamp最大値
    public float smoothSpeed = 5f; // カメラ追従の滑らかさ

    void LateUpdate()
    {
        if (target == null) return;

        // Clampで制限しつつ追従
        float targetX = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float targetY = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);
        Vector3 desiredPos = new Vector3(targetX, targetY, transform.position.z);

        // 滑らかに追従
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
