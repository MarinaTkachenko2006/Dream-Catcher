using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    public Image itemImage;

    [SerializeField]
    public Image borderFrame;

    public event Action<InventoryItem> OnItemClicked;

    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        this.empty = true;
    }

    public void Deselect()
    {
        this.borderFrame.enabled = false;
    }

    public void SetData(Sprite sprite)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.empty = false;
    }

    public void Select()
    {
        borderFrame.enabled = true;
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty)
        {
            return;
        }

        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            OnItemClicked?.Invoke(this);
        }
    }
}

