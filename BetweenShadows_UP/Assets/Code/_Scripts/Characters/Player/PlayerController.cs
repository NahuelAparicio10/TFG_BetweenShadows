using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(StaminaSystem))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerLocomotion _locomotion;
    private PlayerContext _context;
    private WeaponHandler _weaponHandler;
    [SerializeField] private PlayerInteractable _interactable;
    
    private Enums.CharacterState _currentState;
    
    private void Awake()
    {
        var comboController = new ComboController();
        _weaponHandler = GetComponent<WeaponHandler>();
        
        var stats = GetComponent<CharacterStats>();
        var healthSystem = new CharacterHealthSystem();
        var inputs = GetComponent<PlayerInputs>();
        var stamina = GetComponent<StaminaSystem>();
        var animation = GetComponent<PlayerAnimation>();
        var movement = GetComponent<PlayerMovement>();
        var equipmentHandler = GetComponent<EquipmentHandler>();
        var lockOnSystem = GetComponent<PlayerLockOnSystem>();
        
        _context = new PlayerContext
            (
                gameObject,
                stats,
                healthSystem,
                inputs,
                stamina,
                animation,
                comboController,
                movement,
                equipmentHandler,
                lockOnSystem
            );
        
        movement.Initialize(_context);
        _locomotion.Initialize(_context);

        _context.Combo.TryConsumeStamina = _context.Stamina.TryConsumeStamina;
        
        inputs.OnSprint += (pressed) => { _context.Movement.SetSprint(pressed); };
        
        inputs.OnInteract += () =>
        {
            _interactable.InteractPerformed(); 
        };

        inputs.OnAttackInput += AttackInput_perfomed;

        animation.OnInteracting += on =>
        {
            comboController.OnAnimationInteractingChanged(on);
        };
    }

    private void Start()
    {
        _weaponHandler.Initialize(_context);
    }

    private void Update()
    {
        _locomotion.UpdateLocomotion();
        
        _context.Combo.UpdateComboController(_context.Movement.IsGrounded(), _currentState, _context.Animation.PlayTargetAnimation);
    }

    private void FixedUpdate()
    {
        _context.Movement.HandleAllMovement();
    }
    
    public void ChangeCharacterState(Enums.CharacterState newState)
    {
        if(_currentState == newState) return;

        _currentState = newState;
    }

    #region InputEvents
    
    private void AttackInput_perfomed(Enums.AttackInputs atkInput)
    {
        _context.Combo.AddInputToSequence(atkInput, _currentState, _context.Animation.PlayTargetAnimation);
    }
    #endregion
}
