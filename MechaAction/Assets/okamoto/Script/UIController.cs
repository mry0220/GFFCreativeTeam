using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _UIGameOver;
    [SerializeField] private GameObject _UIShop;
    //private Transform canvasSkill;
    [SerializeField] private GameObject _UIFade;
    private Fade _fade;

    private void Awake()
    {
        _fade = _UIFade.GetComponent<Fade>();
    }

    private void Start()
    {
        _UIGameOver.SetActive(false);
        _UIShop.SetActive(false);
    }

    public void GameOver()
    {
        _UIGameOver.SetActive(true);//GameOver‰æ–Ê•\Ž¦
    }

    public void Shop()
    {
        _UIShop.SetActive(true);//‹­‰»‰æ–Ê•\Ž¦
    }

    public void FadeIn()
    {
        _UIFade.SetActive(true);//OnEnable‚ÅActive(true)‚©‚ÂFadeIn
    }

    public void FadeOut()
    {
        _fade.Fade_Out();//FadeOut‚ÅŽ©“®‚ÅActive(false)
    }
}
