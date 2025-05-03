using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField]
    public Image itemImage;

    [SerializeField]
    public Image borderImage;

    public event Action<InventoryItem> OnItemClicked;

    private bool empty = true;
}
