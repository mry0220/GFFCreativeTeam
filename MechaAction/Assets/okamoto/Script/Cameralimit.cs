using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameralimit : MonoBehaviour
{
    private AreaEnemySpawn _spawn;

    [SerializeField]
    public Vector2 cameraMin;
    public Vector2 cameraMax;

    [SerializeField]
    public bool lockX;

    [SerializeField]
    public bool oneTimeTrigger = false;

    [SerializeField]
    public GameObject invisibleWall;

    private bool activated = false;

    private void Start()
    {
        _spawn = GetComponent<AreaEnemySpawn>();

        if (invisibleWall != null)
            invisibleWall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 一度きりモードなら再発動しない
        if (oneTimeTrigger && activated) return;

        // カメラ制限を設定
        GManager.Instance.SetCameraBounds(cameraMin, cameraMax);
        _spawn.StartSpawn();

        invisibleWall.SetActive(true);

        if (lockX && invisibleWall != null)
        {
            


            Debug.Log("敵全滅で解除される予定"); //敵のスポーン処理をつくってから考える


        }

        activated = true;
    }

    public void Clear()
    {
        invisibleWall.SetActive(false);
        GManager.Instance.SetCameraBounds(cameraMin,cameraMax);
    }
}
