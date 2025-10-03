using UnityEngine;
using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly ItemData m_Item;

    public ItemVisual(ItemData item)
    {
        m_Item = item;

        name = $"{m_Item.itemName}";
        style.height = m_Item.SlotDimension.Height * PlayerInventory.SlotDimension.Height;
        style.width = m_Item.SlotDimension.Width * PlayerInventory.SlotDimension.Width;
        style.visibility = Visibility.Hidden;

        VisualElement icon = new VisualElement
        {
            //style = { backgroundImage = m_Item.icon.mainTexture }
        };
        Add(icon);

        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");
    }

    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.top = pos.y;
    }
}