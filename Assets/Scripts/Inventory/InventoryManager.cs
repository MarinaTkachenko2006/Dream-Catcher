using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<Item> collectedItems = new List<Item>();
    public static event Action<Item> OnItemAdded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(Item item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Добавлен предмет: {item.itemName}");
            OnItemAdded?.Invoke(item);
        }
    }


    public List<Item> GetCollectedItems() => new List<Item>(collectedItems);
}
