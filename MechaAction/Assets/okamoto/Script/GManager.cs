using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerAttackSO;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
    [SerializeField] private CameraManager _mainCamera;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerHP_T _playerhp;
    [SerializeField] private SwordHitbox _sword;
    [SerializeField] PowerUpSO _powerup;


    public bool _isAttack;

    private int life = 3;
    public int clear = 1;
    public int score = 0;

    Vector3 currentpoint;
    void Awake()
    {
        if (Instance == null) Instance = this;   //一応
        else Destroy(gameObject);

        _player = GameObject.FindWithTag("Player").transform;

        
    }

    private void Update()
    {
        if(_isAttack)
        {
            _sword._update = _powerup.DamageMult;
        }
        else
        {
            _sword._update = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Reset();
            //Debug.Log("SowdMode");
        }

        Debug.Log(life);
        Debug.Log(clear);

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
        if (life == 0)
        {
            //gameover
            Debug.Log("gameover");
        }

        //フェードアウトさせる

        life--;
        _player.position = currentpoint;
        SetCameraBounds(new Vector2(0,3), new Vector2(1000,5));
        _playerhp.ResetHP();
    }

    public void CheckPoint(Vector3 newPos)
    {
        currentpoint = newPos;
    }

    public void Reset()//gameoverのRetry
    {
        life = 2;
        score = 0;
        //currentpoint 0に戻す（ロードで意味ない）
        //ロードしなおす
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Clear()
    {
        clear++;
        score = 3;
        //UIを出す

        //UIでnextstage Reset();を呼ぶ
        Reset();
       
    }
}
