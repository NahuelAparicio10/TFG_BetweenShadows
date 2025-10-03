using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseAttack", menuName = "Combat/Base Attack")]
public class AttackData : ScriptableObject
{
    [Header("Information:")]
    public string attackName;
    public string animation;
    [Range(0, 1)] public float timeToChangeAnim;
    public float crossFade;
    [Header("Attack Variables:")]
    public Enums.HitType attackHitType;
    public float staminaCost;
    

    [Header("Animation")]
    public AnimationClip animationClip;
    public float AnimationLength => animationClip.length;
    public float TimeToNextCombo => AnimationLength * timeToChangeAnim;

    [Header("States")]
    public List<Enums.CharacterState> AllowedCharacterStates = new List<Enums.CharacterState>();

    public virtual bool Execute(Enums.CharacterState state, Action<string, float> playAnimation)
    {
        if(!IsCharacterInAllowedState(state)) return false;
        playAnimation?.Invoke(animation, crossFade);
        return true;
    }

    private bool IsCharacterInAllowedState(Enums.CharacterState state) => AllowedCharacterStates.Contains(state);
}
