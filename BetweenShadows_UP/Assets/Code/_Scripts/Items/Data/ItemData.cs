using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string uniqueID { get; private set; }
    public Enums.ItemType itemType;
    
    [Header("General Information")] 
    public string itemName;
    public Sprite icon;
    [TextArea] public string description;
    public float weight;
    public int maxStack = 1;

    public Dimensions SlotDimension;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            uniqueID = this.itemName;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}

[Serializable]
public struct Dimensions
{
    public int Height;
    public int Width;
}
