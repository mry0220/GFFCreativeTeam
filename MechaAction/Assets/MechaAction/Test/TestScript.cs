using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    [SerializeField] private TestDB m_DB;

    public void OnAddClick()
    {
        m_DB.AddValue(1);
        m_DB.Toggle(true);
    }

    public void OnClick()
    {
        m_DB.AddValue(1);
        m_DB.Toggle(true);

        SceneManager.LoadScene("SceneB");
    }

    public void DebugClick()
    {
        //Debug.Log(m_DB.Value);
        //Debug.Log(m_DB.Checked);
    }
}
