using System;
using UnityEngine;
public sealed class AnimationEventRelay : MonoBehaviour
{
    public event Action OnHitFrame; // When does the animation will apply damage
    public event Action OnComboWindowOpen; // When the player can execute the next combo
    public event Action OnComboWindowClose;
    public event Action OnIFramesOn; // Activates invulnerability
    public event Action OnIFramesOff;

    private void AE_Hit() => OnHitFrame?.Invoke();
    public void AE_ComboOpen() => OnComboWindowOpen?.Invoke();
    public void AE_ComboClose() => OnComboWindowClose?.Invoke();
    public void AE_IFramesOn() => OnIFramesOn?.Invoke();
    public void AE_IFramesOff() => OnIFramesOff?.Invoke();
}
