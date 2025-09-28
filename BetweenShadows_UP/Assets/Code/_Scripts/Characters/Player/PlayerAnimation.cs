using UnityEngine;
public class PlayerAnimation : CharacterAnimation
{
    [Header("Animation Damping")]
    [SerializeField] private float _inputDamp = 0.12f;
    [SerializeField] private float _speedDamp = 0.10f;

    public void SetInputValuesDamped(Vector2 inputMovement, float dt)
    {
        _animator.SetFloat("dirX", inputMovement.x, _inputDamp, dt);
        _animator.SetFloat("dirY", inputMovement.y, _inputDamp, dt);
    }
    public void SetSpeedDamped(float speedParam, float dt)
    {
        _animator.SetFloat("speed", speedParam, _speedDamp, dt);
    }
    public void SetInputValues(Vector2 inputMovement)
    {
        _animator.SetFloat("dirX", inputMovement.x);
        _animator.SetFloat("dirY", inputMovement.y);
    }

    public void SetSpeed(float speed)
    {
        _animator.SetFloat("speed", speed);
    }
}
