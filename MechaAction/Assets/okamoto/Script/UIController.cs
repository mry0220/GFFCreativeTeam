using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _UIGameOver;
    [SerializeField] private GameObject _UIShop;

    //public Image Fadeimage;
    //public float fadeDuration = 1.0f;

    //private bool isFading = false;
    private void Start()
    {
        _UIGameOver.SetActive(false);
        //_UIShop.SetActive(false);
        //StartCoroutine(FadeIn());
    }

    public void GameOver()
    {
        _UIGameOver.SetActive(true);
    }

    public void Shp()
    {
        _UIShop.SetActive(true);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F) && !isFading)
    //    {
    //        isFading = true;
    //        StartCoroutine(FadeOut());
    //        Debug.Log("フェードアウト");
    //        // new WaitForSeconds(0.5f);
    //    }
    //    if (Input.GetKeyDown(KeyCode.G) && isFading)
    //    {
    //        StartCoroutine(FadeIn());
    //        Debug.Log("フェードイン");
    //        isFading = false;
    //    }
    //}

    //IEnumerator FadeSequence()
    //{
    //isFading = true;
    //yield return StartCoroutine(FadeOut());
    //yield return new WaitForSeconds(0.5f); 
    //yield return StartCoroutine(FadeIn());
    //isFading = false;
    //}

    //public IEnumerator FadeOut()
    //{
        
    //    float timer = 0f;
    //    Color color = Fadeimage.color;
    //    while (timer < fadeDuration)
    //    {
    //        timer += Time.deltaTime;
    //        color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
    //        Fadeimage.color = color;
    //        yield return null;
    //    }
    //}

    //public IEnumerator FadeIn()
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    float timer = 0f;
    //    Color color = Fadeimage.color;
    //    while (timer < fadeDuration)
    //    {
    //        timer += Time.deltaTime;
    //        color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
    //        Fadeimage.color = color;
    //        yield return null;
    //    }
    //}
}
