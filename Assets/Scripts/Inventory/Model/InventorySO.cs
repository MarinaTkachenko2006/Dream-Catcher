using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItemStruct> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 4;

        public event Action<Dictionary<int, InventoryItemStruct>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItemStruct>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public void Clear()
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                inventoryItems[i] = InventoryItemStruct.GetEmptyItem();
            }

            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void AddItem(ItemSO item)
        {
            if (AddItemToFirstFreeSlot(item))
            {
                InformAboutChange();
            }
            else
            {
                Debug.LogWarning("Инвентарь заполнен, не удалось добавить: " + item.name);
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

        public InventoryItemStruct GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public bool ContainsItem(ItemSO item)
        {
            foreach (var inventoryItem in inventoryItems)
            {
                if (!inventoryItem.isEmpty && inventoryItem.item.ID == item.ID)
                    return true;
            }
            return false;
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        private bool AddItemToFirstFreeSlot(ItemSO item)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = new InventoryItemStruct { item = item };
                    return true;
                }
            }
            return false;
        }

    }

    [Serializable]
    public struct InventoryItemStruct
    {
        public ItemSO item;

        public bool isEmpty => item == null;

        public static InventoryItemStruct GetEmptyItem()
            => new InventoryItemStruct
            { item = null };
    }
}