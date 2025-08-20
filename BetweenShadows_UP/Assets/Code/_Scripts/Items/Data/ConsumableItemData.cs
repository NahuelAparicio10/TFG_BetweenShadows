using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Items/ConsumableItemData")]
public class ConsumableItemData : ItemData
{
    public List<BuffEffect> buffs;
}
