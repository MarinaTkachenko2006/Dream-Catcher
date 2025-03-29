using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, Interactable
{
    [SerializeField] private Item itemToGive;
    [SerializeField] private Dialog dialog;
    private bool wasItemGiven = false;

    public void Interact()
    {
        if (!wasItemGiven)
        {
            StartCoroutine(GiveItemAfterDialog());
        }
        else
        {
            Debug.Log("Предмет уже получен.");
        }
    }

    private IEnumerator GiveItemAfterDialog()
    {
        yield return DialogManager.Instance.ShowDialog(dialog);
        InventoryManager.Instance.AddItem(itemToGive);
        wasItemGiven = true;
    }
}
