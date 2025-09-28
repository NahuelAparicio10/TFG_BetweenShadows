using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/WeaponData")]
public class WeaponData : EquipableItemData
{
    public AnimatorOverrideController animatorOverride;

    [Header("Attack Combos")] 
    public AttackData[] lightCombo = new AttackData[4];

    public AttackData[] heavyCombo = new AttackData[4];

}
