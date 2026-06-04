using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_fade;
    [SerializeField] private float m_fadeTime = 1f;

    [SerializeField] private BoolRunTimeSO m_skipFadeIn;

    private void Start()
    {
        if (m_skipFadeIn.Value)
        {
            // フェード無し
            m_fade.alpha = 0;
            m_fade.gameObject.SetActive(false);

            // 次のシーンではフェードするよう戻す
            m_skipFadeIn.SetValue(false);
        }
        else
        {
            // フェードイン
            m_fade.alpha = 1;
            StartCoroutine(FadeIn());
        }
    }

    //Eventで
    public void ChangeScene(string sceneName, bool skipFade)//ここでフェードをスキップできるか決める
    {
        m_skipFadeIn.SetValue(skipFade);

        if (skipFade)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            StartCoroutine(FadeOut(sceneName));
        }
    }

    IEnumerator FadeOut(string scene)
    {
        m_fade.gameObject.SetActive(true);

        float t = 0;

        while (t < m_fadeTime)
        {
            t += Time.deltaTime;
            m_fade.alpha = t / m_fadeTime;
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeIn()
    {
        float t = m_fadeTime;

        while (t > 0)
        {
            t -= Time.deltaTime;
            m_fade.alpha = t / m_fadeTime;
            yield return null;
        }

        m_fade.gameObject.SetActive(false);
    }
}
