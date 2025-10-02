using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] private Image hpBarImage;   // HP–{‘Ì‚ÌImage
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }

    public void ChangeHP(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        UpdateBar();
    }

    private void UpdateBar()
    {
        float percent = currentHealth / maxHealth;
        hpBarImage.fillAmount = percent;
    }

    public void ResetHP()
    {
        currentHealth = maxHealth;
        UpdateBar();
    }
}
