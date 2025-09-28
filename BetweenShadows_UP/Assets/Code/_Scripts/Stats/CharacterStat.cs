using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    public float baseValue;
    public EnumsNagu.StatType statTypeAffected;
    
    private bool _reCalculate;
    private float _value;
    private float _lastBaseValue = float.MinValue;
    private readonly List<StatModifier> _statsModifiers;
    private readonly ReadOnlyCollection<StatModifier> _readStatModifiers;
    
    public float Value
    {
        get
        {
            if (!_reCalculate && baseValue == _lastBaseValue) return _value;
            
            _lastBaseValue = baseValue;
            _value = CalculateFinalValue();
            _reCalculate = false;
            return _value;
        }
    }

    public CharacterStat()
    {
        _statsModifiers = new List<StatModifier>();
        _readStatModifiers = _statsModifiers.AsReadOnly();
    }

    public CharacterStat(float v, EnumsNagu.StatType type) : this()
    {
        baseValue = v;
        statTypeAffected = type;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if(_statsModifiers.Remove(mod))
        {
            _reCalculate = true;
            return true;
        }
        return false;
    }
    
    // - Adds a modifiers to the stat
    public void AddModifier(StatModifier mod)
    {
        _reCalculate = true;
        _statsModifiers.Add(mod);
        _statsModifiers.Sort((a, b) => a.order.CompareTo(b.order));
    }
    
    public bool RemoveAllModifiersFromSource(object source)
    {
        var didRemove = false;
        for(var i = _statsModifiers.Count -1; i>=0; i--)
        {
            if (_statsModifiers[i].source != source) continue;
            
            _reCalculate = true;
            didRemove = true;
            _statsModifiers.RemoveAt(i);
            
        }
        return didRemove;
    }

    public float CalculateFinalValue()
    {
        var finalValue = baseValue;
        float sumPercentAdd = 0;

        for(var i = 0; i < _statsModifiers.Count; i++)
        {
            var mod = _statsModifiers[i];

            switch (mod.type)
            {
                case StatModifierType.Flat:
                    finalValue += mod.value;
                    break;
                case StatModifierType.PercentAdd:
                {
                    sumPercentAdd += mod.value;
                    if (i + 1 >= _statsModifiers.Count || _statsModifiers[i + 1].type != StatModifierType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }

                    break;
                }
                case StatModifierType.PercentMult:
                    finalValue *= 1 + mod.value;
                    break;
                default:
                    Debug.LogWarning("This stat modifiers is not available");
                    break;
            }
        }
        return (float)Math.Round(finalValue, 4);
    }
}
