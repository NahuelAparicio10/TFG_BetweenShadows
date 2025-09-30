using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/WeaponData")]
public class WeaponData : EquipableItemData
{
    public AnimatorOverrideController animatorOverride;

    public List<Combo> combos = new List<Combo>();
}
