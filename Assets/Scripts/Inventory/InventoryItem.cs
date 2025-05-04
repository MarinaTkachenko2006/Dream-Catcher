using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryItem : MonoBehaviour, IPointerClickHandler
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
            itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            borderFrame.enabled = false;
        }

        public void SetData(Sprite sprite)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            empty = false;
        }

        public void Select()
        {
            borderFrame.enabled = true;
        }

        public void OnPointerClick(PointerEventData pointerData)
        {
            if (pointerData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
        }
    }
}