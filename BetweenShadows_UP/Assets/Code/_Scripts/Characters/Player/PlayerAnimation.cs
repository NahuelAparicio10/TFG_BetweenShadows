using System;
using UnityEngine;
public class PlayerAnimation : CharacterAnimation
{
    [Header("Animation Damping")]
    [SerializeField] private float _inputDamp = 0.12f;
    [SerializeField] private float _speedDamp = 0.10f;
    
    static readonly int HashDirX = Animator.StringToHash("dirX");
    static readonly int HashDirY = Animator.StringToHash("dirY");
    static readonly int HashSpeed = Animator.StringToHash("speed");
    static readonly int InteractingBool = Animator.StringToHash("isInteracting");
    
    private bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;
    public event Action<bool> OnInteracting;

    private void LateUpdate()
    {
        SetInteracting();
    }

    private void SetInteracting()
    {
        var currentInteracting = GetInteractingBool();
        
        if (_isInteracting != currentInteracting)
        {
            _isInteracting = currentInteracting;
            if(_isInteracting)
                OnInteracting?.Invoke(true);
            else
            {
                OnInteracting?.Invoke(false);
            }
        }
    }
    public void SetInputValuesDamped(Vector2 inputMovement, float dt)
    {
        _animator.SetFloat(HashDirX, inputMovement.x, _inputDamp, dt);
        _animator.SetFloat(HashDirY, inputMovement.y, _inputDamp, dt);
    }
    public void SetSpeedDamped(float speedParam, float dt)
    {
        _animator.SetFloat(HashSpeed, speedParam, _speedDamp, dt);
    }
    public bool GetInteractingBool() => _animator.GetBool(InteractingBool);
}
