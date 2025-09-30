using System;
using System.Collections.Generic;
using UnityEngine;

public class ComboController
{
    private Combo _activeCombo;
    private AttackData _currentAttack;
    private int _currentStepIndex;
    private float _comboTimer;
    private float _timeToNextCombo;

    private readonly List<Enums.AttackInputs> _currentSequence = new();

    public WeaponData currentWeapon;

    public Transform target;
    public AttackData CurrentAttack => _currentAttack;

    public Func<float, bool> TryConsumeStamina;

    public void OnAnimationInteractingChanged(bool isInteracting)
    {
        Debug.Log($"[Combo] OnAnimationInteractingChanged -> isInteracting: {isInteracting}");
        // C) avoid hard reset on brief toggles; only reset when no attack is currently running
        if (!isInteracting && _currentAttack == null)
            ResetCombo();
    }

    public void UpdateComboController(bool isGrounded, Enums.CharacterState state, Action<string, float> playAnimation)
    {
        if (_currentAttack == null)
        {
            // no active attack this frame
            return;
        }

        _comboTimer += Time.deltaTime;

        // heartbeat while attacking
        Debug.Log($"[Combo] Update | grounded:{isGrounded} step:{_currentStepIndex} seq:[{string.Join(",", _currentSequence)}] " +
                  $"timer:{_comboTimer:0.000} tNext:{_timeToNextCombo:0.000} animLen:{_currentAttack.AnimationLength:0.000}");

        // if is in air or timer done reset
        if (_comboTimer >= _currentAttack.AnimationLength || !isGrounded)
        {
            Debug.Log($"[Combo] Update -> Reset (reason: {(isGrounded ? "clip finished" : "not grounded")})");
            ResetCombo();
            return;
        }

        // Open window to new input; if there is buffered input, continue
        if (_comboTimer >= _timeToNextCombo && HasNextInputBuffered())
        {
            Debug.Log("[Combo] Window open AND next input buffered -> ContinueCombo()");
            ContinueCombo(state, playAnimation);
        }
    }

    public void AddInputToSequence(Enums.AttackInputs action, Enums.CharacterState state, Action<string, float> playAnimation)
    {
        Debug.Log($"[Combo] AddInputToSequence action:{action} state:{state} weaponNull:{(currentWeapon==null)}");

        if (currentWeapon == null || currentWeapon.combos == null || currentWeapon.combos.Count == 0)
        {
            Debug.LogWarning("[Combo] No weapon or combos configured.");
            return;
        }

        // A) if combo is active and buffer already covers all steps, ignore extra inputs until finish
        if (_activeCombo != null && _activeCombo.steps != null && _currentSequence.Count >= _activeCombo.steps.Count)
        {
            Debug.Log("[Combo] Input ignored: buffer already full for this combo.");
            return;
        }

        // If there is no combo active select one
        if (_activeCombo == null)
        {
            Debug.Log("[Combo] No active combo -> searching starting combo...");
            foreach (var combo in currentWeapon.combos)
            {
                if (combo == null || combo.steps == null || combo.steps.Count == 0) continue;
                if (combo.steps[0].input == action)
                {
                    _activeCombo = combo;
                    _currentSequence.Clear();
                    _currentStepIndex = 0;
                    Debug.Log($"[Combo] Selected starting combo with first input:{action} | steps:{combo.steps.Count}");
                    break;
                }
            }
            if (_activeCombo == null)
            {
                Debug.Log("[Combo] No combo starts with this input. Ignored.");
                return;
            }
        }
        else
        {
            // If input doesn't match next step, try to ramify
            if (!_activeCombo.IsNextInput(action, _currentSequence.Count))
            {
                // B) do not attempt fallback if we are already past the last step
                if (_currentSequence.Count >= _activeCombo.steps.Count)
                {
                    Debug.Log("[Combo] Mismatch but buffer is past last step -> ignore input (do not reset).");
                    return;
                }

                Debug.Log($"[Combo] Input doesn't match next step. Trying fallback. " +
                          $"expected:{(_currentSequence.Count < _activeCombo.steps.Count ? _activeCombo.steps[_currentSequence.Count].input.ToString() : "END")} " +
                          $"got:{action}");

                Combo fallback = null;
                foreach (var combo in currentWeapon.combos)
                {
                    if (combo == null || combo.steps == null || combo.steps.Count == 0) continue;
                    var testSeq = new List<Enums.AttackInputs>(_currentSequence) { action };
                    if (combo.MatchesPrefix(testSeq))
                    {
                        fallback = combo;
                        break;
                    }
                }
                if (fallback != null)
                {
                    Debug.Log("[Combo] Fallback combo found -> switching active combo (keeping progress).");
                    _activeCombo = fallback;
                    _currentStepIndex = _currentSequence.Count;
                }
                else
                {
                    Debug.Log("[Combo] No fallback combo matches prefix+input -> ResetCombo()");
                    ResetCombo();
                    return;
                }
            }
        }

        _currentSequence.Add(action);
        Debug.Log($"[Combo] Buffered input -> seq:[{string.Join(",", _currentSequence)}] stepIndex:{_currentStepIndex}");

        // If we haven't executed the current step yet, execute now
        if (_currentAttack == null && _currentSequence.Count - 1 == _currentStepIndex)
        {
            Debug.Log("[Combo] Executing first step immediately.");
            ExecuteAttackAtCurrentStep(state, playAnimation);
            return;
        }

        // If there is an ongoing attack and the window is already open, continue immediately
        if (_currentAttack != null && _comboTimer >= _timeToNextCombo && HasNextInputBuffered())
        {
            Debug.Log("[Combo] Window already open and input buffered -> ContinueCombo()");
            ContinueCombo(state, playAnimation);
        }
    }

    private bool HasNextInputBuffered()
    {
        bool has = _activeCombo != null && _currentSequence.Count > _currentStepIndex + 1;
        Debug.Log($"[Combo] HasNextInputBuffered? {has} (seqCount:{_currentSequence.Count} > step+1:{_currentStepIndex + 1})");
        return has;
    }

    private void ExecuteAttackAtCurrentStep(Enums.CharacterState state, Action<string, float> playAnimation)
    {
        _currentAttack = _activeCombo.GetAttackAt(_currentStepIndex);
        Debug.Log($"[Combo] ExecuteAttack step:{_currentStepIndex} attack:{(_currentAttack ? _currentAttack.name : "NULL")}");

        if (_currentAttack == null)
        {
            Debug.LogWarning("[Combo] ExecuteAttack -> NULL attack. Reset.");
            ResetCombo();
            return;
        }

        bool staminaOK = TryConsumeStamina?.Invoke(_currentAttack.staminaCost) == true;
        Debug.Log($"[Combo] Stamina check -> cost:{_currentAttack.staminaCost} ok:{staminaOK}");
        if (!staminaOK)
        {
            Debug.Log("[Combo] Not enough stamina -> Reset.");
            ResetCombo();
            return;
        }

        _comboTimer = 0f;
        _timeToNextCombo = _currentAttack.TimeToNextCombo;
        Debug.Log($"[Combo] Timers reset. timeToNext:{_timeToNextCombo:0.000} clipLen:{_currentAttack.AnimationLength:0.000}");

        bool executed = _currentAttack.Execute(state, playAnimation);
        Debug.Log($"[Combo] Attack Execute() -> {executed}");
        if (!executed)
        {
            Debug.Log("[Combo] Execute returned false -> Reset.");
            ResetCombo();
        }
    }

    private void ContinueCombo(Enums.CharacterState state, Action<string, float> playAnimation)
    {
        if (!HasNextInputBuffered())
        {
            Debug.Log("[Combo] ContinueCombo called but no buffered input. Ignored.");
            return;
        }

        _currentStepIndex++;
        Debug.Log($"[Combo] Continue -> new stepIndex:{_currentStepIndex}/{_activeCombo?.steps?.Count}");

        if (_activeCombo == null || _currentStepIndex >= _activeCombo.steps.Count)
        {
            Debug.Log("[Combo] Reached end of combo -> Reset.");
            ResetCombo();
            return;
        }
        ExecuteAttackAtCurrentStep(state, playAnimation);
    }

    public void ResetCombo()
    {
        Debug.Log($"[Combo] RESET | prevStep:{_currentStepIndex} prevSeq:[{string.Join(",", _currentSequence)}] " +
                  $"active:{(_activeCombo!=null)} attack:{(_currentAttack? _currentAttack.name : "NULL")}");
        _activeCombo = null;
        _currentAttack = null;
        _currentStepIndex = 0;
        _currentSequence.Clear();
        _comboTimer = 0f;
        _timeToNextCombo = 0f;
    }
}
