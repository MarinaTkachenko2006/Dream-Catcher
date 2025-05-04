using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Inventory;
using Inventory.Model;
using Inventory.UI;

public class InventoryManager1: MonoBehaviour
{
    public static InventoryManager1 Instance;
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
