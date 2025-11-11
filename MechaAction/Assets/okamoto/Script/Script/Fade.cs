using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image Fadeimage; 
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFading)
        {
            isFading = true;
            StartCoroutine(FadeOut());
            Debug.Log("フェードアウト");
            // new WaitForSeconds(0.5f);
        }
        if (Input.GetKeyDown(KeyCode.G) && isFading)
        {
            StartCoroutine(FadeIn());
            Debug.Log("フェードイン");
            isFading =false;
        }
    }

    //IEnumerator FadeSequence()
    //{
        //isFading = true;
        //yield return StartCoroutine(FadeOut());
        //yield return new WaitForSeconds(0.5f); 
        //yield return StartCoroutine(FadeIn());
        //isFading = false;
    //}

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color color = Fadeimage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            Fadeimage.color = color;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = Fadeimage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Fadeimage.color = color;
            yield return null;
        }
    }

}
