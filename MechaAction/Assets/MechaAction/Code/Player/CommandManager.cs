using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CommandManager : MonoBehaviour
{
    [System.Serializable]
    private class Command
    {
        [SerializeField]
        private int skillNumber;
        [SerializeField]
        private string name; //技名
        [SerializeField]
        private List<string> sequence; // 入力手順
        [SerializeField]
        private int maxFrameGap; //入力猶予

        public int SkillNumber => skillNumber;
        
        public string Name => name; //Nameを呼び出したときnameの値を返す

        /*
         public string Name
        {
            get{ return name }
        } と同義
         */
        public List<string> Sequence => sequence;
        public int MaxFrameGap => maxFrameGap;


        public Command(int skillNumber ,string name, List<string> sequence, int maxFrameGap)
        {
            this.skillNumber = skillNumber;
            this.name = name;
            this.sequence = sequence;
            this.maxFrameGap = maxFrameGap;
        }
    }
    [System.Serializable]
    private struct InputData
    {
        public string Input; //　入力内容
        public int Frame; //入力フレーム

        public InputData(string inpug, int frame)
        {
            Input= inpug;
            Frame = frame;
        }
    }

    //InputSystem----------------
    private CommandInput m_action;

    private Vector2 m_move;

    //[SerializeField] private InputActionAsset inputActions; //inputsystem
    [SerializeField] private int bufferLimit = 30; // 入力履歴の格納量
    [SerializeField] private float frameDuration = 60f; //時間管理のフレーム（高フレーム環境でも安定した入力難易度を設定できる
    [SerializeField] private float Frametest;
    private float _frameTime;
    [SerializeField] private List<Command> commandList = new(); //技定義を編集する可能性、複数箇所の使用があるためclass 
    [SerializeField]private List<InputData> _inputBuffer = new(); //履歴を保持するだけだからstruct
    [SerializeField] private int _currentFrame = 0; //入力が行われたフレーム番号を記録するための基準
    private float _frameTimer = 0f; //Time.deltatimeを加算しframeDurationを超えたら_currentframeに加算

    //private InputAction _moveAction; 
    //private InputAction _punchAction;
    //private InputAction _kickAction;
    //private InputAction _strongPunchAction;
    //private InputAction _strongKickAction;

    private int m_jumpBufferFrame;
    private bool m_pendingJump = false;

    private Player m_player;

    [Header("Debug")]
    [SerializeField] private bool m_debug;
    [SerializeField] private DebugCommandManager m_debugCommandManager;

    private void Awake()
    {
        //var map = inputActions.FindActionMap("Player"); //inputActionsAssetに含まれるActionMap Playerを代入

        //_moveAction = map.FindAction("Move"); //それぞれのactionの代入
        //_punchAction = map.FindAction("Punch");
        //_kickAction = map.FindAction("Kick");
        //_strongPunchAction = map.FindAction("StrongPunch");
        //_strongKickAction = map.FindAction("StrongKick");

        //_punchAction.performed += ctx => AddInput("Punch"); //それぞれの入力が入ったら履歴に追加する「処理」を登録
        //_kickAction.performed += ctx => AddInput("Kick");
        //_strongPunchAction.performed += ctx => AddInput("StrongPunch");
        //_strongKickAction.performed += ctx => AddInput("StrongKick");

        m_player = GetComponent<Player>();
     //   QualitySettings.vSyncCount = 0;
     //   Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        m_action = new CommandInput();

        m_action.Player.Move.performed += InputMove;
        m_action.Player.Move.canceled += InputMove;

        m_action.Player.Evade.performed += InputEvade;
        m_action.Player.Light.performed += InputLight;
        m_action.Player.Medium.performed += InputMedium;
        m_action.Player.Heavy.performed += InputHeavy;

        m_action.Enable();

        //_moveAction.Enable();
        //_punchAction.Enable();
        //_kickAction.Enable();
        //_strongPunchAction.Enable();
        //_strongKickAction.Enable();
    }

    private void OnDisable()
    {
        m_action.Disable();

        //_moveAction.Disable();
        //_punchAction.Disable();
        //_kickAction.Disable();
        //_strongPunchAction.Disable();
        //_strongKickAction.Disable();
    }

    private void Start()
    {
        RegisterCommands();
    }

    private void Update()
    {
        AdvanceFrame();
        DetectDirectionalInput();
        CheckCommands();
    }

    private void AdvanceFrame() //現在frameの管理
    {
        _frameTime = 1 / frameDuration;
        _frameTimer += Time.deltaTime;
        while (_frameTimer >= _frameTime) 
        {
            _currentFrame++;
            _frameTimer -= _frameTime;
        }
        Frametest = _currentFrame;
    }

    private void InputMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
    }

    private void InputEvade(InputAction.CallbackContext context)
    {
        AddInput("EV");
    }

    private void InputLight(InputAction.CallbackContext context)
    {
        AddInput("U");
    }

    private void InputMedium(InputAction.CallbackContext context)
    {
        AddInput("I");
    }

    private void InputHeavy(InputAction.CallbackContext context)
    {
        AddInput("O");
    }

    private void DetectDirectionalInput()
    {
        int x;
        int y;

        //Vector2 dir = _moveAction.ReadValue<Vector2>();
        Vector2 dir = m_move;

        if(m_player.Forward.x > 0)
        {
            x = dir.x > 0.5f ? 1 : dir.x < -0.5f ? -1 : 0;
            y = dir.y > 0.5f ? 3 : dir.y < -0.5f ? -3 : 0;
        }
        else
        {
            x = dir.x > 0.5f ? -1 : dir.x < -0.5f ? 1 : 0;
            y = dir.y > 0.5f ? 3 : dir.y < -0.5f ? -3 : 0;
        }

        int num = 5+x+y;

        if(y == 3)
        {
            if(!m_pendingJump)
            {
                m_jumpBufferFrame = 4;
                m_pendingJump = true;
            }
        }

        if(m_player.actionState == Player.EnumActionState.Run)
        {
            if(num == 5 || num == 4 || num == 7)
            {
                m_player.OnResetRun();
            }
        }

       // Debug.Log(_currentFrame);
        if(num >= 1 && num <= 9 )//&& num != 5
        AddInput(num.ToString());
    }//入力方向の管理

    private string m_bInput;

    private void AddInput(string input) //入力履歴の管理
    {
        if(m_debug)
        {
            m_debugCommandManager.OnUpdateCommandView(input, _currentFrame);
        }
        //入力した瞬間をinputBufferに　２回入力はNurtralになる（6,5,6)
        if(m_bInput == input) return;

        m_bInput = input;

        _inputBuffer.Add(new InputData(input, _currentFrame));
        if(input != "5")
         //Debug.Log($"入力:{input} Frame: {_currentFrame}");

        if (_inputBuffer.Count > bufferLimit)
            _inputBuffer.RemoveAt(0);
    }

    private void RegisterCommands() //コマンド技の登録
    {
        commandList.Add(new Command(1, "Reload", new List<string> { "2", "5", "2", "U" }, 10));
        commandList.Add(new Command(2,"Hadouken", new List<string> { "2", "3", "6", "U"}, 10));
        commandList.Add(new Command(3,"Shouryuken", new List<string> { "6", "2", "3", "U" }, 10));
        commandList.Add(new Command(4,"Tatsumakisenpukyaku", new List<string> { "2", "1", "4", "I" }, 10));
        commandList.Add(new Command(5,"TyrantRave", new List<string> { "6", "3", "2","1","4","6", "U" }, 10));
        commandList.Add(new Command(6,"Shinkuuhadouken", new List<string> { "2", "3", "6","2","3","6", "I" }, 10));
        commandList.Add(new Command(7,"Shinkuutatumakisenpukyaku", new List<string> { "2", "1", "4","2","1","4", "I" }, 10));
        commandList.Add(new Command(8,"GyakuyogaFlame", new List<string> { "6", "3", "2","1","4", "O" }, 10));
        commandList.Add(new Command(9,"irukasan", new List<string> { "4", "4", "4","4","6", "O" }, 10));
        commandList.Add(new Command(10,"Run", new List<string> { "6","5","6" }, 8));
        commandList.Add(new Command(11, "Evade", new List<string> { "EV" }, 1));
        commandList.Add(new Command(12, "UpperAttack", new List<string> { "8","U" }, 4));
        commandList.Add(new Command(13, "Attack", new List<string> { "U" }, 10));

        //commandList.Add(new Command(14, "Jump", new List<string> { "8" }, 1));

    }

    private void CheckCommands() // 技出力内容を管理
    {
        foreach(var cmd  in commandList) //配列やリストを順に調べるループ文
        {
            if (MatchCommand(cmd))
            {
                switch(cmd.SkillNumber)
                {
                    case 1: 
                        //m_player.CallSlash();
                        break;
                    case 2: m_player.OnHadouken();
                        break;
                    case 3:
                        m_player.OnShouryuken();
                        break;
                    case 10:
                        m_player.OnRun();
                        Debug.Log("Run");
                        break;
                    case 11:
     
                        m_player.OnEvade();
                        Debug.Log("EV");
                        break;
                    case 12:
                    
                        Debug.Log("upperAttack");

                        break;
                    case 13:
                        m_player.OnNormalAttack();

                        break;
                    //case 14:
                    //    m_player.CallJump();
                    //    break;
                }
                //Debug.Log($"技発動:{cmd.Name} Frame: {_currentFrame}");
                _inputBuffer.Clear(); //履歴の初期化
                break;
            }
        }

        // コマンドが成立しなかった場合 jumpはここで
        CheckPendingInput();
    }

    private bool MatchCommand(Command cmd)
    {
        int step = 0;
        int lastFrame = -1;

        for (int i = 0; i < _inputBuffer.Count; i++)
        {
            var data = _inputBuffer[i];
            string expected = cmd.Sequence[step];

            //間に別のが挟まった場合コマンドを無効にする
            //if(step > 0 && data.Input != expected)
            //{

            //}

            if (data.Input == expected)
            {
                if (step > 0)
                {
                    int frameGap = data.Frame - lastFrame;
                    if (frameGap > cmd.MaxFrameGap)
                    {
                        step = 0;
                        lastFrame = -1;
                        continue;
                    }
                }

                lastFrame = data.Frame;
                step++;
                //  Debug.Log($"入力: Input: {data.Input},Fame: {data.Frame} step: {step}");


                if (step >= cmd.Sequence.Count)
                {
                    //  Debug.Log($"入力: Input: {data.Input},Fame: {data.Frame} step: {step}");

                    return true;
                }
            }
            else
            {
                continue;
            }
        }

        return false;
    }

    private void CheckPendingInput()
    {
        // 8が入力されたが、
        // まだ8Pになる可能性がある
        if (m_jumpBufferFrame > 0)
        {
            m_jumpBufferFrame--;
            return;
        }

        // 受付時間が終わった
        if (m_jumpBufferFrame <= 0 && m_pendingJump)
        {
            m_pendingJump = false;
            m_player.CallJump();
        }
    }
}
