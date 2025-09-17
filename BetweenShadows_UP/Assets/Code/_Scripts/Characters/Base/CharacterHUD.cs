using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterHUD
{
    [SerializeField] protected Image _lifeBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _lifeBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
