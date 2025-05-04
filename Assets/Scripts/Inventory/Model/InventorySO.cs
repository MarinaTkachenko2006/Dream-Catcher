using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItemStruct> inventoryItems;

    [field: SerializeField]
    public int Size { get; private set; } = 4;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItemStruct>();
        for (int i = 0; i < Size; i++)
        {
            inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
        }
    }

    public void AddItem(ItemSO item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                inventoryItems[i] = new InventoryItemStruct
                {
                    item = item
                };
            }
        }
    }

    public Dictionary<int, InventoryItemStruct> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItemStruct> returnValue = 
            new Dictionary<int, InventoryItemStruct>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].isEmpty)
            {
                continue;
            }
            returnValue[i] = inventoryItems[i];
        }
        return returnValue;
    }
}

[Serializable]
public struct InventoryItemStruct
{
    public ItemSO item;

    public bool isEmpty => item != null;

    public static InventoryItemStruct GetEmptyItem()
        => new InventoryItemStruct
        { item = null }; 
}