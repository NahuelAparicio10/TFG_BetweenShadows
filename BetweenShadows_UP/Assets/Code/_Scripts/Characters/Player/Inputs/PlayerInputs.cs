using System;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private InputService _input;
    private PlayerInputActions InputActions => _input.Actions;
    public InputBuffer Buffer { get; private set; } = new InputBuffer(0.25f);

    private void Awake()
    {
        _input = GameServices.Get<InputService>();
        SubscribeToInputEvents();
    }
    private void SubscribeToInputEvents()
    {
        InputActions.Player.Dodge.performed += _ => Buffer.Enqueue(new DodgeCmd());
        InputActions.Player.Interact.performed += _ => Buffer.Enqueue(new InteractCmd());
        InputActions.Player.LightAttack.performed += _ => Buffer.Enqueue(new LightAttack());
        InputActions.Player.HeavyAttack.performed += _ => Buffer.Enqueue(new HeavyAttackCmd());
    }
    private Vector2 GetDirection() => InputActions.Player.Move.ReadValue<Vector2>();
    public Vector3 GetDirectionNormalized() => UtilsNagu.GetCameraForwardNormalized(Camera.main) * GetDirection().y + UtilsNagu.GetCameraRightNormalized(Camera.main) * GetDirection().x;

}
