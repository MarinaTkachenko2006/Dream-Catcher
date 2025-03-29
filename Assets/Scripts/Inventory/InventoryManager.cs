using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private List<Item> allItems;
    private HashSet<Item> collectedItems = new HashSet<Item>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Добавлен предмет: {item.itemName}");
            // Можно добавить событие для обновления UI: OnItemAdded?.Invoke(item);
        }
    }

    public bool HasItem(Item item) => collectedItems.Contains(item);

    public List<Item> GetCollectedItems() => new List<Item>(collectedItems);
}
