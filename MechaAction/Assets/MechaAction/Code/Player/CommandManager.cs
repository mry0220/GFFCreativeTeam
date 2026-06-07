using System.Collections.Generic;
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
        private string name; //�Z��
        [SerializeField]
        private List<string> sequence; // ���͎菇
        [SerializeField]
        private int maxFrameGap; //���͗P�\

        public int SkillNumber => skillNumber;
        
        public string Name => name; //Name���Ăяo�����Ƃ�name�̒l��Ԃ�

        /*
         public string Name
        {
            get{ return name }
        } �Ɠ��`
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
        public string Input; //�@���͓��e
        public int Frame; //���̓t���[��

        public InputData(string inpug, int frame)
        {
            Input= inpug;
            Frame = frame;
        }
    }

    [SerializeField] private InputActionAsset inputActions; //inputsystem
    [SerializeField] private int bufferLimit = 30; // ���͗����̊i�[��
    [SerializeField] private float frameDuration = 60f; //���ԊǗ��̃t���[���i���t���[�����ł����肵�����͓�Փx��ݒ�ł���
    [SerializeField] private float Frametest;
    private float _frameTime;
    [SerializeField] private List<Command> commandList = new(); //�Z��`��ҏW����\���A�����ӏ��̎g�p�����邽��class 
    [SerializeField]private List<InputData> _inputBuffer = new(); //������ێ����邾��������struct
    private int _currentFrame = 0; //���͂��s��ꂽ�t���[���ԍ����L�^���邽�߂̊
    private float _frameTimer = 0f; //Time.deltatime�����Z��frameDuration�𒴂�����_currentframe�ɉ��Z

    private InputAction _moveAction; 
    private InputAction _punchAction;
    private InputAction _kickAction;
    private InputAction _strongPunchAction;
    private InputAction _strongKickAction;

    private Player m_player;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player"); //inputActionsAsset�Ɋ܂܂��ActionMap Player����

        _moveAction = map.FindAction("Move"); //���ꂼ���action�̑��
        _punchAction = map.FindAction("Punch");
        _kickAction = map.FindAction("Kick");
        _strongPunchAction = map.FindAction("StrongPunch");
        _strongKickAction = map.FindAction("StrongKick");

        _punchAction.performed += ctx => AddInput("Punch"); //���ꂼ��̓��͂��������痚���ɒǉ�����u�����v��o�^
        _kickAction.performed += ctx => AddInput("Kick");
        _strongPunchAction.performed += ctx => AddInput("StrongPunch");
        _strongKickAction.performed += ctx => AddInput("StrongKick");

        m_player = GetComponent<Player>();
     //   QualitySettings.vSyncCount = 0;
     //   Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _punchAction.Enable();
        _kickAction.Enable();
        _strongPunchAction.Enable();
        _strongKickAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _punchAction.Disable();
        _kickAction.Disable();
        _strongPunchAction.Disable();
        _strongKickAction.Disable();
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

    private void AdvanceFrame() //����frame�̊Ǘ�
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

    private void DetectDirectionalInput()
    {
        Vector2 dir = _moveAction.ReadValue<Vector2>();
        int x = dir.x > 0.5f ? 1 : dir.x < -0.5f ? -1 : 0;
        int y = dir.y > 0.5f ? 3 : dir.y < -0.5f ? -3 : 0;
        int num = 5+x+y;
       // Debug.Log(_currentFrame);
        if(num >= 1 && num <= 9 )//&& num != 5
        AddInput(num.ToString());
    }//���͕����̊Ǘ�

    private void AddInput(string input) //���͗����̊Ǘ�
    {
        _inputBuffer.Add(new InputData(input, _currentFrame));
        if(input != "5")
        //Debug.Log($"����:{input} Frame: {_currentFrame}");

        if(_inputBuffer.Count > bufferLimit)
            _inputBuffer.RemoveAt(0);
    }

    private void RegisterCommands() //�R�}���h�Z�̓o�^
    {
        commandList.Add(new Command(1, "Reload", new List<string> { "2", "5", "2", "Punch" }, 10));
        commandList.Add(new Command(2,"Hadouken", new List<string> { "2", "3", "6", "Punch"}, 10));
        commandList.Add(new Command(3,"Shouryuken", new List<string> { "6", "2", "3", "Punch" }, 10));
        commandList.Add(new Command(4,"Tatsumakisenpukyaku", new List<string> { "2", "1", "4", "Kick" }, 10));
        commandList.Add(new Command(5,"TyrantRave", new List<string> { "6", "3", "2","1","4","6", "Punch" }, 10));
        commandList.Add(new Command(6,"Shinkuuhadouken", new List<string> { "2", "3", "6","2","3","6", "StrongPunch" }, 10));
        commandList.Add(new Command(7,"Shinkuutatumakisenpukyaku", new List<string> { "2", "1", "4","2","1","4", "StrongKick" }, 10));
        commandList.Add(new Command(8,"GyakuyogaFlame", new List<string> { "6", "3", "2","1","4", "StrongPunch" }, 10));
        commandList.Add(new Command(9,"irukasan", new List<string> { "4", "4", "4","4","6", "StrongPunch" }, 10));
        commandList.Add(new Command(10,"Attack", new List<string> { "Punch" }, 10));

    }

    private void CheckCommands() // �Z�o�͓��e���Ǘ�
    {
        foreach(var cmd  in commandList) //�z��⃊�X�g�����ɒ��ׂ郋�[�v��
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
                    case 5:
                        m_player.OnShouryuken();
                        break;
                    case 10:
                        m_player.OnNormalAttack();
                        break;
                }
                //Debug.Log($"�Z����:{cmd.Name} Frame: {_currentFrame}");
                _inputBuffer.Clear(); //�����̏�����
                break;
            }
        }
    }

    private bool MatchCommand(Command cmd)
    {
        int step = 0;
        int lastFrame = -1;

        for (int i = 0; i < _inputBuffer.Count; i++)
        {
            var data = _inputBuffer[i];
            string expected = cmd.Sequence[step];

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
                      //  Debug.Log($"����: Input: {data.Input},Fame: {data.Frame} step: {step}");

                if (step >= cmd.Sequence.Count)
                {
                      //  Debug.Log($"����: Input: {data.Input},Fame: {data.Frame} step: {step}");
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

}
