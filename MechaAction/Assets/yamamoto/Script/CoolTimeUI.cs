using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    public GameObject CircularUI;
    public Image CooldownImage;

    private float cooldownTime = 3f;
    private float timer = 0f;
    private bool isCooling = false;

    private void Start()
    {
        CircularUI.SetActive(false);
        CooldownImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isCooling)
        {
            CircularUI.SetActive(true);
            timer = cooldownTime;
            isCooling = true;
            CooldownImage.fillAmount = 1f;
        }

        if (isCooling)
        {
            timer -= Time.deltaTime;
            CooldownImage.fillAmount = timer / cooldownTime;

            if (timer <= 0f)
            {
                isCooling = false;
                CircularUI.SetActive(false);
                CooldownImage.fillAmount = 0f;
            }
        }
    }
}