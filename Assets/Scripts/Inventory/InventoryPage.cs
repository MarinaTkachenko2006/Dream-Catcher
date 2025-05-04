using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField]
        private InventoryItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        [SerializeField]
        private InventoryDescription itemDescription;

        List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public event Action<int> OnDescriptionRequested;

        private void Awake()
        {
            Hide();
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(contentPanel);
                inventoryItems.Add(item);
                item.OnItemClicked += HandleItemSelection;
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage)
        {
            if (inventoryItems.Count > itemIndex)
            {
                inventoryItems[itemIndex].SetData(itemImage);
            }
        }

        private void HandleItemSelection(InventoryItem item)
        {
            int index = inventoryItems.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription();
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (InventoryItem item in inventoryItems)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        internal void UpdateDescription(int itemIndex, Sprite icon, string name, string description)
        {
            itemDescription.SetDescription(icon, name, description);
            DeselectAllItems();
            inventoryItems[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach(InventoryItem item in inventoryItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}