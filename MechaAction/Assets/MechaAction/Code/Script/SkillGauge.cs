using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillGauge : MonoBehaviour
{
    [SerializeField] private Image hpBarImage;   // HP–{‘Ì‚ÌImage
    private GameObject _player;
    private Player_AttackT _playerattack;
    private float _skillmax;
    private float _currentskill;

    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null)
        {
            _playerattack = _player.GetComponent<Player_AttackT>();
        }
        //currentHealth = maxHealth;

    }

    private void Start()
    {
        _skillmax = _playerattack.SkillMax;
        _currentskill = _playerattack.Currentskill;
        UpdateBar();
    }

    private void Update()
    {
        _currentskill = _playerattack.Currentskill;
        UpdateBar();
    }

    //public void ChangeHP(float value)
    //{

    //    currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
    //    UpdateBar();
    //}

    private void UpdateBar()
    {
        float percent = _currentskill / _skillmax;
        hpBarImage.fillAmount = percent;
    }

    //public void ResetHP()
    //{
    //    currentHealth = maxHealth;
    //    UpdateBar();
    //}
}
