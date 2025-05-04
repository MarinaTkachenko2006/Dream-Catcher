using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Inventory;
using Inventory.Model;
using Inventory.UI;

public class InventoryManager_1 : MonoBehaviour
{
    public static InventoryManager_1 Instance;

    /// <summary>
    /// ������� ���������� ������ ���, ����� ��������� ���������.
    /// </summary>
    public event UnityAction OnInventoryChanged;

    [Header("������ �� UI � ������")]
    [SerializeField]
    private InventoryController inventoryController; // ����������� ���������� � ����������

    private InventorySO inventoryData;

    private void Awake()
    {
        // ��������, ��� � ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // �������� �����. ������ �� �����������
        inventoryData = inventoryController.GetComponent<InventoryController>()
                            .GetType()
                            .GetField("inventoryData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            .GetValue(inventoryController) as InventorySO;

        // ������������� � �������� �� ���������� �������
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += HandleInternalUpdate;

        // ����������� �������������� ��������, ���� �����
        foreach (var structItem in inventoryController.initialItems)
        {
            if (!structItem.isEmpty)
                inventoryData.AddItem(structItem.item);
        }
    }

    private void HandleInternalUpdate(Dictionary<int, InventoryItemStruct> state)
    {
        // ��������� ������� ������
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// ������ AddItem(string) � ���� SO-������ �� ����� � ���������.
    /// </summary>
    public void AddItem(string itemName)
    {
        ItemSO itemSO = Resources.Load<ItemSO>($"Assets/ScriptableObjects/{itemName}");
        if (itemSO == null)
        {
            Debug.LogWarning($"[InventoryManager2] ItemSO � ������ '{itemName}' �� ������ � Resources/Items");
            return;
        }

        inventoryData.AddItem(itemSO);
    }

    public int ItemsCount()
    {
        return inventoryData
            .GetCurrentInventoryState()
            .Count(kv => !kv.Value.isEmpty);
    }


    public bool HasItem(string itemName)
    {
        return inventoryData
            .GetCurrentInventoryState()
            .Values
            .Any(s => !s.isEmpty && s.item.Name == itemName);
    }


    public void Reset()
    {
        inventoryData.Clear();
        OnInventoryChanged?.Invoke();
    }
}
