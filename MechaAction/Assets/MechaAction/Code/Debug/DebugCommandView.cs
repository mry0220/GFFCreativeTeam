using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugCommandViewData
{
    public Sprite CurrentInputImage;
    public Sprite CurrentDirImage;
    public int CurrentFrame;
}

public class DebugCommandView : MonoBehaviour
{
    

    private DebugCommandViewData m_data;

    [SerializeField] private Image m_inputImage;
    [SerializeField] private Image m_dirImage;
    [SerializeField] private TMP_Text m_text;

    public DebugCommandViewData Data => m_data;

    private void Start()
    {
        m_inputImage.enabled = false;
        m_dirImage.enabled = false;
        m_text.text = null;

        m_data = new();
    }

    public void SetInputImage(Sprite image)
    {
        if(image == null)
        {
            m_inputImage.sprite = null;
            m_inputImage.enabled = false;

            m_data.CurrentInputImage = null;
            return;
        }

        if(m_inputImage.enabled == false)
        {
            m_inputImage.enabled = true;
        }
        m_inputImage.sprite = image;
        m_data.CurrentInputImage = image;
    }

    public void SetDirImage(Sprite image)
    {
        if (image == null)
        {
            m_dirImage.sprite = null;
            m_dirImage.enabled = false;

            m_data.CurrentDirImage = null;
            return;
        }


        if (m_dirImage.enabled == false)
        {
            m_dirImage.enabled = true;
        }
        m_dirImage.sprite = image;
        m_data.CurrentDirImage = image;
    }

    public void SetText(int frame)
    {
        if (frame == 0) return;

        if(frame > 99)
        {
            frame = 99;
        }

        m_text.text = frame.ToString();
        m_data.CurrentFrame = frame;
    }

    public void SetData(DebugCommandViewData data)
    {

        SetInputImage(data.CurrentInputImage);

        SetDirImage(data.CurrentDirImage);

        SetText(data.CurrentFrame);
    }
}
