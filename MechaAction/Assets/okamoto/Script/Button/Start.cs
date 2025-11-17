using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void OnButton()
    {
        SceneManager.LoadScene("StageScene");
    }
}
