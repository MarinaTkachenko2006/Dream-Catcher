using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Color highlightColor = Color.magenta;

    public static InventoryUI Instance { get; private set; }

    private List<Button> itemSlots = new List<Button>();
    private int selectedIndex = -1;

    void Start()
    {
        Debug.Log("InventoryUI Start() вызван!");

        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel НЕ УСТАНОВЛЕН в Inspector!");
            return;
        }

        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Нажата I");
            ToggleInventory();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (inventoryPanel != null)
        {
            bool wasActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(true);
            itemsContainer = inventoryPanel.transform.Find("InventoryContent");
            inventoryPanel.SetActive(wasActive);

            if (itemsContainer == null)
            {
                Debug.LogError("InventoryContent не найден!");
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (itemsContainer == null && inventoryPanel != null)
        {
            bool wasActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(true);
            itemsContainer = inventoryPanel.transform.Find("InventoryContent");
            inventoryPanel.SetActive(wasActive);
        }

        UpdateInventoryUI();
    }


    public void UpdateInventoryUI()
    {
        if (itemsContainer == null)
        {
            Debug.LogError("InventoryContent не найден!");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager не инициализирован!");
            return;
        }

        if (itemSlotPrefab == null)
        {
            Debug.LogError("ItemSlotPrefab не назначен!");
            return;
        }

        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        itemSlots.Clear();

        List<ItemSO> collectedItems = InventoryManager.Instance.GetCollectedItems();

        // Проверка на пустой инвентарь
        if (collectedItems == null || collectedItems.Count == 0)
        {
            Debug.Log("Нет предметов в инвентаре!");
            return;
        }

        for (int i = 0; i < collectedItems.Count; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer);
            Button button = slot.GetComponent<Button>();

            Transform iconTransform = slot.transform.Find("Icon");
            if (iconTransform == null)
            {
                Debug.LogError("Icon не найден в префабе!");
                continue;
            }
            Image icon = slot.transform.Find("Icon")?.GetComponent<Image>();

            Transform highlightTransform = slot.transform.Find("HighlightFrame");
            Image highlight = slot.transform.Find("HighlightFrame")?.GetComponent<Image>();

            if (icon == null || highlight == null)
            {
                Debug.LogError("Не найдены иконка или рамка в префабе!");
                continue;
            }

            icon.sprite = collectedItems[i].icon;
            int index = i;

            button.onClick.AddListener(() => SelectItem(index));
            itemSlots.Add(button);
        }
    }

    private void SelectItem(int index)
    {
        List<ItemSO> collectedItems = InventoryManager.Instance.GetCollectedItems();

        if (index < 0 || index >= collectedItems.Count) return;

        selectedIndex = index;

        // обновляем описание
        selectedItemIcon.sprite = collectedItems[index].icon;
        itemDescriptionText.text = $"<b>{collectedItems[index].Name}</b>\n{collectedItems[index].Description}";

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Transform slot = itemSlots[i].transform;
            Image highlight = slot.Find("HighlightFrame").GetComponent<Image>();
            if (highlight != null)
                highlight.enabled = (i == index); // включаем только для выбранного
        }
    }

    public void ToggleInventory()
    {
        Debug.Log($"Перед переключением: {inventoryPanel.activeSelf}");
        
        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel не установлен!");
            return;
        }

        bool newState = !inventoryPanel.activeSelf;
        Debug.Log($"Инвентарь: {newState} (Перед изменением)");

        Debug.Log($"После переключения: {inventoryPanel.activeSelf}");

        inventoryPanel.SetActive(newState);

        Debug.Log($"Инвентарь: {inventoryPanel.activeSelf} (После изменения)");

        if (newState)
        {
            UpdateInventoryUI();
            inventoryPanel.transform.SetAsLastSibling();
        }
    }
}
