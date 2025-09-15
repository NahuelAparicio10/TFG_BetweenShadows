using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private PlayerInputActions _actions;
    public event Action OnDodge;
    public event Action OnLightAttack;
    public event Action OnHeavyAttack;
    public event Action OnInteract; 

    private void Awake()
    {
        _actions = new PlayerInputActions();
        _actions.Enable();
        SubscribeToInputEvents();
    }
    private void SubscribeToInputEvents()
    {
        _actions.Player.Dodge.performed += _ => OnDodge?.Invoke();
        _actions.Player.Interact.performed += _ => OnInteract?.Invoke();
        _actions.Player.LightAttack.performed += _ => OnLightAttack?.Invoke();
        _actions.Player.HeavyAttack.performed += _ => OnHeavyAttack?.Invoke();
    }
    private Vector2 GetDirection() => _actions.Player.Move.ReadValue<Vector2>();
    public Vector3 GetDirectionNormalized() => Utils.GetCameraForwardNormalized(Camera.main) * GetDirection().y + Utils.GetCameraRightNormalized(Camera.main) * GetDirection().x;
    private void UnSuscribeToInputEvents()
    {
        _actions.Disable();
    }
    private void OnDestroy()
    {
        UnSuscribeToInputEvents();
    }
}
