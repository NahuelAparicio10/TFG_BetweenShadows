using System;
using UnityEngine;

[RequireComponent(typeof(PlayerHUD))]
public class PlayerController : CharacterBase
{
    private CharacterHealthSystem _healthSystem;
    [SerializeField] private PlayerHUD _hud;
    
    protected override void Awake()
    {
        base.Awake();
        _healthSystem = GetComponent<CharacterHealthSystem>();
    }

    protected override void Start()
    {
        base.Start();
        InitializeSubscriptions();
    }

    private void InitializeSubscriptions()
    {
        _healthSystem.OnHealthChanged += HealthChanged;
    }
    
    

    private void HealthChanged(float current, float max)
    {
        _hud.UpdateHealthBar(current, max);
    }

    private void OnDestroy()
    {
        _healthSystem.OnHealthChanged -= HealthChanged;

    }
}
