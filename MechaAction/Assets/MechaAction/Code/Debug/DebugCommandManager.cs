using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommandManager : MonoBehaviour
{
    public enum CommandType
    {
        up,
        upright,
        right,
        downright,
        down,
        downleft,
        left,
        upleft,
        N,
        U,
        I,
        O,
        J,
        SW,
        EV,
    }

    [System.Serializable]
    public class DebugCommandImageData
    {
        public CommandType m_type;
        public Sprite m_sprite;
    }

    [SerializeField] private List<DebugCommandImageData> m_imageData = new();

    //CommandManagerから情報を貰う
    //現在のTextにフレームを更新
    //List管理で　古いのを下にコピー
    [SerializeField] private List<DebugCommandView> m_viewObj;

    private int m_inputframe;

    private Sprite m_inputSprite;
    private Sprite m_dirSprite;

    private string m_currentInput;

    private bool m_inputCommand =false;

    //if 1F InputCommand,  U,U,U  not Update Frame
    private bool m_inputFrame = false;

    public void OnUpdateCommandView(string input, int frame)
    {
        switch(input)
        {
            case "Punch":
                m_inputSprite = m_imageData.Find(x => x.m_type == CommandType.U).m_sprite;
                m_dirSprite = null;
                m_inputCommand = true;
                break;
        }

        if(m_inputCommand == true)
        {
            m_inputCommand = false;

            //フラグで連続してInput系の物が来たとき更新するように
            if (m_inputFrame == true)
            {
                for (int i = m_viewObj.Count - 1; i > 0; i--)
                {
                    m_viewObj[i].SetData(m_viewObj[i - 1].Data);
                }

                m_viewObj[0].SetInputImage(m_inputSprite);
                m_viewObj[0].SetDirImage(m_dirSprite);
                m_currentInput = null;


                return;
            }

            m_viewObj[0].SetInputImage(m_inputSprite);
            m_currentInput = null;

            

            m_inputFrame = true;

            return;
        }

        if(m_currentInput != input)
        {
            m_currentInput = input;
            m_inputFrame = false;

            m_inputSprite = null;

            switch (input)
            {
                case "1":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.downleft).m_sprite;
                    break;
                case "2":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.down).m_sprite;
                    break;
                case "3":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.downright).m_sprite;
                    break;
                case "6":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.right).m_sprite;
                    break;
                case "9":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.upright).m_sprite;
                    break;
                case "8":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.up).m_sprite;
                    break;
                case "7":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.upleft).m_sprite;
                    break;
                case "4":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.left).m_sprite;
                    break;
                case "5":
                    m_dirSprite = m_imageData.Find(x => x.m_type == CommandType.N).m_sprite;
                    break;
                
            }

            //m_newData = new DebugCommandViewData()
            //{
            //    CurrentInputImage = 
            //}

            m_inputframe = frame;

            for (int i = m_viewObj.Count - 1; i > 0; i--)
            {
                m_viewObj[i].SetData(m_viewObj[i - 1].Data);
            }

            m_viewObj[0].SetInputImage(m_inputSprite);
            m_viewObj[0].SetDirImage(m_dirSprite);
            m_viewObj[0].SetText(1);

        }
        else
        {
            int nowframe = frame - m_inputframe;

            OnUpdateFrame(nowframe);
        }

        
    }

    private void OnUpdateFrame(int frame)
    {
        m_viewObj[0].SetText(frame);
    }
}
