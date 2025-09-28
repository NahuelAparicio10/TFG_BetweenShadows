using System;
using UnityEngine;
using System.Collections.Generic;

// Manages all the base stats for any character

[Serializable]
public class CharacterStats : MonoBehaviour
{
    [SerializeField] private List<CharacterStat> _baseStats = new List<CharacterStat>();
    private Dictionary<EnumsNagu.StatType, CharacterStat> _statsDict;
    
    private void Awake()
    {
        _statsDict = new Dictionary<EnumsNagu.StatType, CharacterStat>();
        foreach (var entry in _baseStats)
        {
            _statsDict.TryAdd(entry.statTypeAffected, entry);
        }
    }
    
    // Returns the value of the stat
    public float GetStatValue(EnumsNagu.StatType type)
    {
        if (_statsDict.TryGetValue(type, out var stat))
        {
            return stat.Value;
        }
        Debug.LogWarning($"Stat {type} not found in {name}");
        return 0;
    }
    
    // Returns the CharacterStat object so you can add modifiers
    public CharacterStat GetStat(EnumsNagu.StatType type)
    {
        if (_statsDict.TryGetValue(type, out var stat))
        {
            return stat;
        }

        Debug.LogWarning($"Stat {type} not found in {name}");
        return null;
    }
    
    // Adds a modifier to the specific stat
    public void AddModifier(EnumsNagu.StatType type, StatModifier modifier)
    {
        if (_statsDict.TryGetValue(type, out var stat))
            stat.AddModifier(modifier);
        else
            Debug.LogWarning($"Stat {type} not found in {name}");
    }
    
    // Deletes a modifier from a stat
    public void RemoveModifier(EnumsNagu.StatType type, StatModifier modifier)
    {
        if (_statsDict.TryGetValue(type, out var stat))
            stat.RemoveModifier(modifier);
    }
    
    // Deletes all the modifiers from source (ex. temporal buff or debuff)
    public void RemoveModifiersFromSource(object source)
    {
        foreach (var stat in _statsDict.Values)
            stat.RemoveAllModifiersFromSource(source);
    }
}
