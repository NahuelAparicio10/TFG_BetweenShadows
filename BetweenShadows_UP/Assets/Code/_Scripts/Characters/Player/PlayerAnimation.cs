using UnityEngine;
public class PlayerAnimation : CharacterAnimation
{


    public void SetInputValues(Vector2 inputMovement)
    {
        _animator.SetFloat("InputX", inputMovement.x);
        _animator.SetFloat("InputY", inputMovement.y);
    }

    public void SetSpeed(float speed)
    {
        _animator.SetFloat("speed", speed);
    }
}
