using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _UIGameOver;

    private void Start()
    {
        _UIGameOver.SetActive(false);
    }

    public void GameOver()
    {
        _UIGameOver.SetActive(true);
    }
}
