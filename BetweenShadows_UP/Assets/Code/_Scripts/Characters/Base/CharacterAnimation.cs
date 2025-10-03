using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    protected Animator _animator;
    public Animator Animator => _animator;
    public event Action OnHitFrame; // When does the animation will apply damage
    public event Action OnComboWindowOpen; // When the player can execute the next combo
    public event Action OnComboWindowClose; // Player won't be able to continue with the next combo (time passed)
    public event Action OnIFramesOn; // Activates invulnerability
    public event Action OnIFramesOff;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.Fixed;
    }
    
    public virtual void PlayTargetAnimation(string targetAnimation, float timeToFade)
    {
        _animator.CrossFade(targetAnimation, timeToFade);
    }
    
    public void AE_Hit() => OnHitFrame?.Invoke();
    public void AE_ComboOpen() => OnComboWindowOpen?.Invoke();
    public void AE_ComboClose() => OnComboWindowClose?.Invoke();
    public void AE_IFramesOn() => OnIFramesOn?.Invoke();
    public void AE_IFramesOff() => OnIFramesOff?.Invoke();
}
