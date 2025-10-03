using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipableItemData", menuName = "Items/EquipableItemData")]
public class EquipableItemData : ItemData
{
    public Enums.EquipSlot equipSlot;
    public List<StatModifier> modifiers;
    public GameObject prefab; 
}
