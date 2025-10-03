using UnityEngine;
public class ItemStack
{
    public ItemData data { get; private set; }
    public int quantity { get; private set; }
    
    public ItemStack(ItemData data, int quantity = 1)
    {
        this.data = data;
        this.quantity = Mathf.Clamp(quantity, 0, data.maxStack);
    }
    
    public bool IsEmpty => quantity <= 0;
    
    public void Add(int amount)
    {
        quantity = Mathf.Clamp(quantity + amount, 0, data.maxStack);
    }

    public void Remove(int amount)
    {
        quantity = Mathf.Max(quantity - amount, 0);
    }

    public bool Use(GameObject user)
    {
        switch (data.itemType)
        {
            case Enums.ItemType.Consumable:
                return TryUseConsumable(user);
            // case Enums.ItemType.Equipable:
            //     return TryEquip(user);

            // Otros casos como coleccionables o crafteables pueden simplemente no tener uso directo
            default:
                Debug.LogWarning($"Item of type {data.itemType} cannot be used.");
                return false;
        }
    }

    private bool TryUseConsumable(GameObject user)
    {
        if (data is not ConsumableItemData consumable)
            return false;

        // var buffHandler = user.GetComponent<BuffHandler>();
        // if (buffHandler == null)
        // {
        //     Debug.LogWarning("User has no BuffHandler.");
        //     return false;
        // }
        //
        // foreach (var buff in consumable.buffs)
        // {
        //     buffHandler.ApplyBuff(buff);
        // }

        Remove(1);
        return true;
    }

    /*private bool TryEquip(GameObject user)
    {
        if (data is not EquipableItemData equipable)
            return false;

        var equipmentHandler = user.GetComponent<EquipmentHandler>();
        if (equipmentHandler == null)
        {
            Debug.LogWarning("User has no EquipmentHandler.");
            return false;
        }
        
        bool equipped = equipmentHandler.Equip(equipable);
        if (equipped)
            Remove(1);

        return equipped;
    }*/
}
