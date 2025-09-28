using System;

// -- Buff effect base class, defines the basic parameters for a buff
[Serializable]
public class BuffEffect
{
    public EnumsNagu.StatType statType;
    public StatModifierType modifierType;
    public float baseValue;
    public float duration;
    
    [NonSerialized] public StatModifier runtimeModifier;
}