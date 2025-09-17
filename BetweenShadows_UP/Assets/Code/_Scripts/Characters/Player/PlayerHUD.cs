using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerHUD : CharacterHUD
{
    [SerializeField] private Image _stamina;

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        _stamina.fillAmount = Mathf.Clamp01(currentStamina / maxStamina);
    }
    
}
