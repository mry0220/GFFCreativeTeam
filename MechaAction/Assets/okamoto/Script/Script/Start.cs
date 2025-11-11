using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void OnStatButtonPressed()
    {
        SceneManager.LoadScene("gameScene");
    }
}
