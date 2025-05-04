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
    /// —обытие вызываетс€ каждый раз, когда инвентарь изменилс€.
    /// </summary>
    public event UnityAction OnInventoryChanged;

    [Header("—сылки на UI и данные")]
    [SerializeField]
    private InventoryController inventoryController; // об€зательно проставить в инспекторе

    private InventorySO inventoryData;

    private void Awake()
    {
        // синглтон, как в оригинале
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
        // получаем внутр. данные из контроллера
        inventoryData = inventoryController.GetComponent<InventoryController>()
                            .GetType()
                            .GetField("inventoryData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            .GetValue(inventoryController) as InventorySO;

        // инициализаци€ и подписка на внутреннее событие
        inventoryData.Initialize();
        inventoryData.OnInventoryUpdated += HandleInternalUpdate;

        // скармливаем первоначальные элементы, если нужно
        foreach (var structItem in inventoryController.initialItems)
        {
            if (!structItem.isEmpty)
                inventoryData.AddItem(structItem.item);
        }
    }

    private void HandleInternalUpdate(Dictionary<int, InventoryItemStruct> state)
    {
        // форвардим событие наружу
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// јналог AddItem(string) Ц ищем SO-объект по имени и добавл€ем.
    /// </summary>
    public void AddItem(string itemName)
    {
        ItemSO itemSO = Resources.Load<ItemSO>($"Assets/ScriptableObjects/{itemName}");
        if (itemSO == null)
        {
            Debug.LogWarning($"[InventoryManager2] ItemSO с именем '{itemName}' не найден в Resources/Items");
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
