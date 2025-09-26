using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string uniqueID { get; private set; }
    public EnumsNagu.ItemType itemType;
    
    [Header("General Information")] 
    public string itemName;
    public Image icon;
    [TextArea] public string description;
    public float weight;
    public int maxStack = 1;

    private void OnValidate()
    {
        #if UNITY_EDITOR
            uniqueID = this.itemName;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
