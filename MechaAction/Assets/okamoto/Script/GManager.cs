using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
    [SerializeField] private CameraManager _mainCamera;

    public int life = 3;
    public int clear = 1;

    Vector3 currentpoint;
    void Awake()
    {
        if (Instance == null) Instance = this;   //一応
        else Destroy(gameObject);
    }

    // Clamp値を変更
    public void SetCameraBounds(Vector2 min, Vector2 max) //カメラ制限
    {
        _mainCamera.minPos = min;
        _mainCamera.maxPos = max;
    }

    public void OnPlayerHit() //プレイヤーがダメージを受けたとき
    {
        if (_mainCamera != null)
            _mainCamera.ShakeCamera();   // カメラ揺らす
    }

    public void DiePlayer()
    {
        life--;
        //currentpointへ移動 Player.position == currentpoint;
        //HPを戻す、残機を減らす GetComponent<Player_HP>PlayerHP.ReturnHP();
        //フェードアウトさせる
        //gameover画面
    }

    public void CheckPoint(Vector3 newPos)
    {
        currentpoint = newPos;
    }

    public void Reset()
    {
        life = 3;
    }
}
