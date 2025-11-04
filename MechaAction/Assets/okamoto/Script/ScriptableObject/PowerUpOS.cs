using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUp")]
public class PowerUpOS : ScriptableObject
{
    [Header("アタック倍率")] public float damagepower = 1.2f;
}
