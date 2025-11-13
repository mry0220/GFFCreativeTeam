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
    private GameObject _playerobj;
    private Transform _playerposition;
    private Player _player;
    private PlayerHP _playerhp;
    [SerializeField] PowerUpSO _powerup;
    public bool _isAttack;

    private int life = 1;
    public int clear = 1;
    public int score = 0;

    public Vector3 currentpoint;
    private Vector3 _startPosition = new Vector3(0f,3.5f,0f);
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
        
        currentpoint = _startPosition;
        SceneManager.sceneLoaded += SceneLoaded;
        //参照をここにもってくる


    }

    private void Start()
    {
        
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

    public IEnumerator DiePlayer()
    {
        _player.Dead();//Vector3.zeroにするため　後消す
        if (life == 0)
        {
            _ui.GameOver();
            Debug.Log("gameover");
            yield break;
        }

        _ui.FadeIn();                                                  //フェードインさせる
        yield return new WaitForSeconds(1.5f);

        _player._ChangeState(PlayerState.Respawn);                     //一旦respawnに
        life--;                                                        //life-1
        SetCameraBounds(new Vector2(0,3), new Vector2(1000,5));
        _playerposition.position = currentpoint;
        StartCoroutine(_playerhp.ResetHP());
        _ui.FadeOut();
        yield return new WaitForSeconds(1f);

        _player._ReturnNormal();

        yield return new WaitForSeconds(2f);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
           LayerMask.NameToLayer("Enemy"), false);

        yield break;
    }

    public void CheckPoint(Vector3 newPos)
    {
        currentpoint = newPos;
    }

    public void Reset()//gameoverのRetry
    {
        life = 2;
        score = 0;//一応
        currentpoint = _startPosition;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
         LayerMask.NameToLayer("Enemy"), false);//一応
        Debug.Log("Reset");//呼ばれた
        //ロードしなおす
    }

    public void Clear()
    {
        //スコアを保存
        score = 0;
        //強化UIを出す
        _ui.Shop();
    }

    public void NextStage()
    {
        clear++;
        Reset();
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)//シーンがロードされると呼ばれる
    {
        _mainCamera = FindFirstObjectByType<CameraManager>();
        _playerobj = GameObject.FindWithTag("Player");
        if (_playerobj != null)
        {
            _player = _playerobj.GetComponent<Player>();
            _playerposition = _playerobj.transform;
            _playerhp = _playerobj.GetComponent<PlayerHP>();
            currentpoint = _playerobj.transform.position;
        }
        _ui = FindFirstObjectByType<UIController>();

        if (nextScene.name == "StageScene")
        {
            _playerposition.position = currentpoint;
            life = 0;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
          LayerMask.NameToLayer("Enemy"), false);
        }

        
    }
}
