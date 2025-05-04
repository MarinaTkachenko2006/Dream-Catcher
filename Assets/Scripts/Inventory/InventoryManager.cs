using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private List<ItemSO> collectedItems = new List<ItemSO>();
    public static event Action<ItemSO> OnItemAdded;

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

    public void AddItem(ItemSO item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Добавлен предмет: {item.Name}");
            OnItemAdded?.Invoke(item);
        }
    }


    public List<ItemSO> GetCollectedItems() => new List<ItemSO>(collectedItems);
}
