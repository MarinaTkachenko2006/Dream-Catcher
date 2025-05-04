using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager: MonoBehaviour
{
    public static InventoryManager Instance;
    public event UnityAction OnInventoryChanged;

    void Awake()
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

    private HashSet<string> items = new HashSet<string>();
    public void AddItem(string itemName)
    {
        items.Add(itemName);
        OnInventoryChanged?.Invoke();
    }

    public int ItemsCount()
    {
        return items.Count;
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }

    public void Reset()
    {
        items.Clear();
        OnInventoryChanged?.Invoke();
    }

}
