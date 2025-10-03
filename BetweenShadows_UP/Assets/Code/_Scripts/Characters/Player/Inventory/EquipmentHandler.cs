using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EquipmentHandler : MonoBehaviour
{
    [System.Serializable]
    public class EquipSlotBinding
    {
        public Enums.EquipSlot slot;
        public Transform socket;
    }
    
    [SerializeField] private List<EquipSlotBinding> _bindings = new();
    private CharacterStats _stats;

    private readonly Dictionary<Enums.EquipSlot, EquipableItemData> _equipped = new();
    private readonly Dictionary<Enums.EquipSlot, GameObject> _spawned = new();

    public event Action<Enums.EquipSlot, EquipableItemData> OnEquipped;
    public event Action<Enums.EquipSlot, EquipableItemData> OnUnequipped;
    
    private void Awake()
    {
        _stats = GetComponent<CharacterStats>();
    }
    public bool Equip(EquipableItemData item)
    {
        if (item == null) return false;
        var slot = item.equipSlot;

        // If there is anything equipped, unequip.
        if (_equipped.ContainsKey(slot))
        {
            Unequip(slot);
        }
        
        _equipped[slot] = item;

        // If there is equipment prefab insantiate
        if (item.prefab != null)
        {
            var parent = GetSocket(slot) ?? transform;
            var go = Instantiate(item.prefab, parent, false);
            _spawned[slot] = go;
        }

        // Apply stat modifieres to stats such as + defendes + attack or whatever :P
        
        if (_stats != null && item.modifiers != null)
        {
            foreach (var m in item.modifiers)
            {
                var clone = new StatModifier(m.value, m.type, m.order, item);
                _stats.AddModifier(m.statTypeAffected, clone);
            }
        }
        OnEquipped?.Invoke(slot, item);
        return true;
    }
    
    public bool Unequip(Enums.EquipSlot slot)
    {
        if (!_equipped.TryGetValue(slot, out var item)) return false;

        // Clean the stats added from the last weapon
        _stats.RemoveModifiersFromSource(item);

        // Destroy the equip prefab
        if (_spawned.TryGetValue(slot, out var go) && go != null)
        {
            Destroy(go);
            _spawned.Remove(slot);
        }

        _equipped.Remove(slot);
        OnUnequipped?.Invoke(slot, item);
        return true;
    }
    
    public EquipableItemData Get(Enums.EquipSlot slot)
    {
        _equipped.TryGetValue(slot, out var i);
        return i;
    }

    public WeaponData GetWeapon(Enums.EquipSlot slot = Enums.EquipSlot.Weapon) => Get(slot) as WeaponData;

    private Transform GetSocket(Enums.EquipSlot slot) => _bindings.Find(b => b.slot == slot)?.socket;
}
