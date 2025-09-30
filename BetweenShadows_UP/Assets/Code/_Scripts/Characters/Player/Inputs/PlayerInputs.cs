using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private float _stickDeadzone = 0.18f;
    
    private InputService _input;
    private PlayerInputActions InputActions => _input.Actions;
    //  public InputBuffer Buffer { get; private set; } = new InputBuffer(0.25f);
    public event Action<Enums.AttackInputs> OnAttackInput;
    public event Action<bool> OnSprint;
    public event Action OnDodge; 
    public event Action OnInteract; 
    public event Action OnLockOn;

    private Transform _mainCameraTransform;
    
    private void Awake()
    {
        _input = GameServices.Get<InputService>();
        _mainCameraTransform = Camera.main.transform;
        SubscribeToInputEvents();
    }
    private void SubscribeToInputEvents()
    {
        InputActions.Player.Dodge.performed += OnDodge_performed;
        InputActions.Player.Interact.performed += OnInteract_performed;
        InputActions.Player.LightAttack.performed += OnLightAttack_perfomed;
        InputActions.Player.HeavyAttack.performed += OnHeavyAttack_perfomed;
        InputActions.Player.Sprint.performed += Sprint_performed;
        InputActions.Player.Sprint.canceled += Sprint_canceled;
    }

    private void UnSuscribeToInputEvents()
    {
        InputActions.Player.Dodge.performed -= OnDodge_performed;
        InputActions.Player.Interact.performed -= OnInteract_performed;
        InputActions.Player.LightAttack.performed -= OnLightAttack_perfomed;
        InputActions.Player.HeavyAttack.performed -= OnHeavyAttack_perfomed;
        InputActions.Player.Sprint.performed -= Sprint_performed;
        InputActions.Player.Sprint.canceled -= Sprint_canceled;
    }

    private void OnLightAttack_perfomed(InputAction.CallbackContext ctx) { OnAttackInput?.Invoke(Enums.AttackInputs.LightInput); }
    private void OnHeavyAttack_perfomed(InputAction.CallbackContext ctx) { OnAttackInput?.Invoke(Enums.AttackInputs.HeavyInput); }
    private void OnInteract_performed(InputAction.CallbackContext ctx) { OnInteract?.Invoke(); }
    private void OnDodge_performed(InputAction.CallbackContext ctx) { OnDodge?.Invoke(); }
    private void Sprint_performed(InputAction.CallbackContext ctx) { OnSprint?.Invoke(true); }
    private void Sprint_canceled(InputAction.CallbackContext ctx) { OnSprint?.Invoke(false); }

    public float RawMag2() => GetDirectionNormalized().sqrMagnitude;
    public bool HasRaw() => RawMag2() > (_stickDeadzone * _stickDeadzone);
    
    public Vector2 GetDirection() => InputActions.Player.Move.ReadValue<Vector2>();
    public Vector3 GetDirectionNormalized() => UtilsNagu.GetCameraForwardNormalized(_mainCameraTransform) * GetDirection().y + UtilsNagu.GetCameraRightNormalized(_mainCameraTransform) * GetDirection().x;


    private void OnDestroy()
    {
        UnSuscribeToInputEvents();
    }
}
