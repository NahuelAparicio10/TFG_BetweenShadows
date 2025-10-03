using System;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Stadistics:")]
    public float timeToStartRecovering = 1f;
    [SerializeField] private float _recoveryRate = 10f;       
    [SerializeField] private float _recoveryAmountPerSecond = 10f;       
    [SerializeField] private float _consumeRate = 1.5f;
    [SerializeField] private float _consumptionAmountPerSecond = 1.5f;

    private float _currentStamina;
    private float _recoveryTimer;
    private bool _isConsumingStamina = false;
    public System.Action OnStaminaModifies;
    
    private CharacterStat _maxStamina;
    
    #region Properties
    public float MaxStamina => _maxStamina.Value;
    public float CurrentStamina => _currentStamina;
    #endregion
    
    private void Start()
    {
        _maxStamina = GetComponent<CharacterStats>().GetStat(Enums.StatType.Stamina);
        _currentStamina = _maxStamina.Value;
    }

    private void Update()
    {
        if (_isConsumingStamina)
        {
            ConsumeStamina();
        }
        else
        {
            HandleStaminaRecover();
        }
    }
    public bool TryConsumeStamina(float cost)
    {
        if (!HasStaminaToAction(cost)) return false;
        OnConsumeStamina(cost);
       // hud.UpdateStamina(_currentStamina, _maxStamina.Value);
        return true;
    }
    public virtual void OnConsumeStamina(float amount)
    {
        _currentStamina = Mathf.Max(_currentStamina - amount, 0f);
        _recoveryTimer = 0;
        OnStaminaModifies?.Invoke();
    }

    private void HandleStaminaRecover()
    {
        if(_currentStamina >= _maxStamina.Value || _isConsumingStamina) return;

        _recoveryTimer += Time.deltaTime;

        if (_recoveryTimer >= timeToStartRecovering)
        {
            ModifyStamina(_recoveryAmountPerSecond, _recoveryRate);
        }
    }
    
    private void ConsumeStamina()
    {
        if (_currentStamina > 0)
        {
            ModifyStamina(-_consumptionAmountPerSecond, _consumeRate);
        }

        if (_currentStamina <= 0)
        {
            _recoveryTimer = 0;
        }
    }

    public void StartConsumingStamina()
    {
        if (_isConsumingStamina || _currentStamina <= 0) return;

        _isConsumingStamina = true;
        _recoveryTimer = 0;
    }

    public void StopConsumingStamina() => _isConsumingStamina = false;

    protected virtual void ModifyStamina(float value, float rate)
    {
        _currentStamina += (value / rate) * Time.deltaTime;
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina.Value);
        OnStaminaModifies?.Invoke();
    }

    public bool HasStaminaToAction(float cost) => _currentStamina >= cost;
}
