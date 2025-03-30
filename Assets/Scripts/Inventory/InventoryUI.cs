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
        Debug.Log("InventoryUI Start() ������!");

        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel �� ���������� � Inspector!");
            return;
        }

        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("������ I");
            ToggleInventory();
        }
    }

    private void Awake()
    {
        itemsContainer = inventoryPanel.transform.Find("InventoryContent");

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        bool wasActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(true);

        itemsContainer = inventoryPanel.transform.Find("InventoryContent");
        
        inventoryPanel.SetActive(wasActive);

        if (itemsContainer == null)
        {
            Debug.LogError("ItemsContent �� ������ � ������ ���������!");
            return;
        }

        UpdateInventoryUI();
    }


    public void UpdateInventoryUI()
    {
        if (itemsContainer == null)
        {
            Debug.LogError("InventoryContent �� ������!");
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager �� ���������������!");
            return;
        }

        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        itemSlots.Clear();

        List<Item> collectedItems = InventoryManager.Instance.GetCollectedItems();

        // �������� �� ������ ���������
        if (collectedItems == null || collectedItems.Count == 0)
        {
            Debug.Log("��� ��������� � ���������!");
            return;
        }

        for (int i = 0; i < collectedItems.Count; i++)
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer);
            Button button = slot.GetComponent<Button>();
            Image icon = slot.transform.Find("Icon").GetComponent<Image>(); // ���� ������
            Image highlight = slot.transform.Find("HighlightFrame")?.GetComponent<Image>(); // �����

            if (icon == null || highlight == null)
            {
                Debug.LogError("�� ������� ������ ��� ����� � ������� �����!");
                continue;
            }

            icon.sprite = collectedItems[i].icon;
            int index = i; // ��� ��������� � ������

            button.onClick.AddListener(() => SelectItem(index));
            itemSlots.Add(button);
        }
    }

    private void SelectItem(int index)
    {
        List<Item> collectedItems = InventoryManager.Instance.GetCollectedItems();

        if (index < 0 || index >= collectedItems.Count) return;

        selectedIndex = index;

        // ��������� ��������
        selectedItemIcon.sprite = collectedItems[index].icon;
        itemDescriptionText.text = $"<b>{collectedItems[index].itemName}</b>\n{collectedItems[index].description}";

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Transform slot = itemSlots[i].transform;
            Image highlight = slot.Find("HighlightFrame").GetComponent<Image>();
            if (highlight != null)
                highlight.enabled = (i == index); // �������� ������ ��� ����������
        }
    }

    public void ToggleInventory()
    {
        Debug.Log($"����� �������������: {inventoryPanel.activeSelf}");
        
        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel �� ����������!");
            return;
        }

        bool newState = !inventoryPanel.activeSelf;
        Debug.Log($"���������: {newState} (����� ����������)");

        Debug.Log($"����� ������������: {inventoryPanel.activeSelf}");

        inventoryPanel.SetActive(newState);

        Debug.Log($"���������: {inventoryPanel.activeSelf} (����� ���������)");

        if (newState)
        {
            UpdateInventoryUI();
            inventoryPanel.transform.SetAsLastSibling();
        }
    }
}
