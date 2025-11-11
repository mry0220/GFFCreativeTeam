using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void OnButton()
    {
        SceneManager.LoadScene("Title");
    }
}
