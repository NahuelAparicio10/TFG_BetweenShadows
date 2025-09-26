using UnityEngine;

public class EnumsNagu
{
    #region Items & Inventory
    public enum ItemType { Equipable, Consumable, Crafting, Collectable }
    public enum EquipSlot { Weapon, Helmet, ChestArmor, LowerArmor, Backpack, Necklace, Ring }
    #endregion
    
    public enum StatType { Health, Stamina, Speed, Defense, Attack, Weight }
    
    public enum HitType { Normal, Knockback, Knockdown }
    
}
