using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DirectionTarget : MonoBehaviour
{
    public Transform CurrentTarget => currentTarget;
    private Transform currentTarget;// 現在のターゲット
    private GameObject TargetUI;
    private Image image;
    private RectTransform rectTransform;
    private Camera _cam;

    private float _switchmouse = 3f;
    private Vector3 lastMousePos;

    private void Start()
    {
        TargetUI = GameObject.Find("Target");
        if(TargetUI != null )
        {
            image = TargetUI.GetComponent<Image>();
            rectTransform = TargetUI.GetComponent<RectTransform>();
        }
        else
        {
            Debug.Log("Target null");
        }
        _cam = Camera.main;
        lastMousePos = Input.mousePosition;

        if(image != null) image.enabled = false;
    }

    private void Update()
    {
        if (currentTarget == null)
            FindTarget();
        if (PlayerMovedMouse())
            SwitchTarget();
        LockonTarget();
    }

    private bool PlayerMovedMouse() //AI
    {
        // Input.GetAxis はプレイヤー操作のみ検知
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
            return true;

        return false;
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach(GameObject e in enemies)
        {
            // 画面内かチェック
            Vector3 screenPos = _cam.WorldToScreenPoint(e.transform.position);
            if (screenPos.x < 0 || screenPos.x > Screen.width ||
    screenPos.y < 0 || screenPos.y > Screen.height ||
    screenPos.z < 0)
                continue; // 画面外は無視

            // 距離計算
            float dist = Vector2.Distance(transform.position,e.transform.position);
            if(dist < minDist)
            {
                Debug.Log("Find Target");
                minDist = dist;
                nearest = e.transform;
            }
        }
        
        currentTarget = nearest;
    }

    private void SwitchTarget()
    {
        // ターゲットがいない or 画面内に1体しかいなければ切替処理をしない
        if (!currentTarget) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length <= 1) return;

        // マウスの移動方向
        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        if (mouseDelta.magnitude < _switchmouse) return;

        Vector2 direction = mouseDelta.normalized;

        // 現在のターゲットから見て「マウス方向に最も近い敵」を選ぶ
        float bestDot = -1f;
        Transform bestTarget = null;

        foreach(GameObject e in enemies)
        {
            if (e.transform == currentTarget) continue;

            Vector2 toEnemy = (e.transform.position - currentTarget.position).normalized;
            float dot = Vector2.Dot(direction, toEnemy);

            if (dot > bestDot)
            {
                bestDot = dot;
                bestTarget = e.transform;
            }
        }

        if( bestTarget != null && bestDot > 0f) currentTarget = bestTarget;
    }

    void LockonTarget()
    {
        if (currentTarget != null)
        {
            // 自動でマウスを敵に合わせる
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(currentTarget.position);

            if (image != null) image.enabled = true;
            //Vector2 targetPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, currentTarget.transform.position);
            rectTransform.position = targetScreenPos;
        }
        else
        {
            if (image != null) image.enabled = false;
        }
    }
}
