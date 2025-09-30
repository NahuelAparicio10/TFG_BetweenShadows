using UnityEngine;

public class Enums
{
    #region Items & Inventory
    public enum ItemType { Equipable, Consumable, Crafting, Collectable }
    public enum EquipSlot { Weapon, Helmet, ChestArmor, LowerArmor, Backpack, Necklace, Ring }
    #endregion
    
    public enum StatType { Health, Defense, Stamina, Speed, Attack, Weight }
    
    public enum CharacterState { Moving, Attacking, Dodge, Stuned, Dead }
    public enum AttackInputs { LightInput, HeavyInput }
    public enum HitType { Normal, Knockback, Knockdown }
}
