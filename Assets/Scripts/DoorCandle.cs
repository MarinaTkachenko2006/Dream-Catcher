using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCandle : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("бида");
            return;
        }

        UpdateCandleState();
    }

    public void UpdateCandleState()
    {
        bool hasItem = InventoryManager.Instance.HasItem(itemName);
        spriteRenderer.sprite = hasItem ? activeSprite : inactiveSprite;
    }

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += UpdateCandleState;
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= UpdateCandleState;
        }
    }
}
