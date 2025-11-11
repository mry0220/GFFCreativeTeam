using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField]
    private Text TimerUI;

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
        TimerUI.GetComponent<Text>().text = timer.ToString("F1");
    }
}