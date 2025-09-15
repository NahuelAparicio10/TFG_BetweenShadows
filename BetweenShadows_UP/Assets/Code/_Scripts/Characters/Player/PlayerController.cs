using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : CharacterBase
{
    private CharacterHealthSystem _healthSystem;
    private PlayerInputs _inputs;
    private PlayerMovement _movement;
    [SerializeField] private PlayerHUD _hud;
    
    protected override void Awake()
    {
        base.Awake();
        _inputs = GetComponent<PlayerInputs>();
        _healthSystem = GetComponent<CharacterHealthSystem>();
    }

    protected override void Start()
    {
        base.Start();
        InitializeSubscriptions();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        _movement.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
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
