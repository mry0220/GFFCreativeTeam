using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameralimit : MonoBehaviour
{
    private AreaEnemySpawn _spawn;
    private BossSpawn _bossSpawn;

    [SerializeField]
    public Vector2 cameraMin;
    public Vector2 cameraMax;
    public Vector2 cameraMinRE;
    public Vector2 cameraMaxRE;

    [SerializeField]
    public GameObject invisibleWall;

    private bool activated = false;

    public bool _bosscheck;

    private void Start()
    {
        if (_bosscheck == false)
            _spawn = GetComponent<AreaEnemySpawn>();
        else
            _bossSpawn = GetComponent<BossSpawn>();

        if (invisibleWall != null)
            invisibleWall.SetActive(false);

        GManager.Instance.AreaTrigger(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (activated) return;

        // ÉJÉÅÉâêßå¿Çê›íË
        GManager.Instance.SetCameraBounds(cameraMin, cameraMax);
        if (_bosscheck == false)
            _spawn.StartSpawn();
        else
            _bossSpawn.StartSpawn();

        invisibleWall.SetActive(true);

        activated = true;
    }

    public void Clear()
    {
        invisibleWall.SetActive(false);
        GManager.Instance.SetCameraBounds(cameraMinRE,cameraMaxRE);
    }

    public void DeadClear()
    {
        activated = false;
    }
}
