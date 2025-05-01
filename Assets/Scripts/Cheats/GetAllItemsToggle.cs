using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllItemsToggle : MonoBehaviour
{
    [SerializeField] CheatManager cheatManager;

    public void GiveItems(bool toggleValue)
    {
        if (toggleValue)
        {
            cheatManager.GetAllItems();
        }
        else
        {
            cheatManager.GetAllItems();
        }
    }
}
