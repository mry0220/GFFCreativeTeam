using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void OnStatButtonPressed()
    {
        SceneManager.LoadScene("Title");
    }
}
