using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
　　private CameraManager _mainCamera;
    private UIController _ui;
    private GameObject _player;
    private Transform _playerposition;
    private PlayerHP _playerhp;
    [SerializeField] PowerUpSO _powerup;
    public bool _isAttack;

    private int life = 0;
    public int clear = 1;
    public int score = 0;

    Vector3 currentpoint;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;   //一応
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += SceneLoaded;
        //参照をここにもってくる


    }

    private void Start()
    {
        //_mainCamera = FindFirstObjectByType<CameraManager>();
        //_player = GameObject.FindWithTag("Player");
        //if( _player != null )
        //{
        //    _playerposition = _player.transform;
        //    _playerhp = _player.GetComponent<PlayerHP>();
        //    currentpoint = _player.transform.position;
        //}
        //GameObject[] uiObjects = GameObject.FindGameObjectsWithTag("UI");
        //_UIGameOver = uiObjects.FirstOrDefault(obj => obj.name == "GameOver");
        //_UIGameOver.SetActive(false);
        //var _GameUI = uiObjects.FirstOrDefault(obj => obj.name == "GameUI");
    }

    private void Update()
    {
        //if(_isAttack)
        //{
        //    _sword._update = _powerup.DamageMult;
        //}
        //else
        //{
        //    _sword._update = 1.0f;
        //}


        if (Input.GetKeyDown(KeyCode.O))
        {
            Reset();
            //Debug.Log("SowdMode");
        }

        //Debug.Log(life);
        //Debug.Log(clear);

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
            _ui.GameOver();
            Debug.Log("gameover");
        }

        //フェードアウトさせる

        life--;
        _playerposition.position = currentpoint;
        SetCameraBounds(new Vector2(0,3), new Vector2(1000,5));
        StartCoroutine(_playerhp.ResetHP());
    }

    public void CheckPoint(Vector3 newPos)
    {
        currentpoint = newPos;
    }

    public void Reset()//gameoverのRetry
    {
        life = 2;
        clear++;
        score = 0;
        //currentpoint 0に戻す（ロードで意味ない）
        //ロードしなおす
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Clear()
    {
        clear++;
        score = 3;
        //強化UIを出す

        //UIでnextstage Reset();を呼ぶ
        Reset();
       
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)//シーンがロードされると呼ばれる
    {
        if(nextScene.name == "StageScene")
        {
            life = 0;
        }

        _mainCamera = FindFirstObjectByType<CameraManager>();
        _player = GameObject.FindWithTag("Player");
        if( _player != null )
        {
            _playerposition = _player.transform;
            _playerhp = _player.GetComponent<PlayerHP>();
            currentpoint = _player.transform.position;
        }
        _ui = FindFirstObjectByType<UIController>();
        if( _ui != null )
        {
            Debug.Log("ui");
        }
        else
        {
            Debug.Log("ui_unll");
        }
    }
}
