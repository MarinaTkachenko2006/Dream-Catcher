using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        itemDescription.SetDescription(image, title, description);
        inventoryItems[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        inventoryItems[0].SetData(image);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
