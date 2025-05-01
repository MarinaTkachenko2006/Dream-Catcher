using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiverController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog1;
    [SerializeField] Dialog dialog2;
    [SerializeField] private Dialog dialogFirstItem;
    public string ItemToGive;
    //public bool itemIsGiven = false;
    AudioManager audioManager;
    InventoryManager inventoryManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        inventoryManager = InventoryManager.Instance;
    }
    public void Interact()
    {
        if (!inventoryManager.HasItem(ItemToGive))
        {
            StartCoroutine(InteractRoutine());
        }
        else
        {
            Debug.Log("Предмет уже получен");
        }
    }

    IEnumerator InteractRoutine()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog1));
        audioManager.PlaySFX(audioManager.item);
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog2));
        if (inventoryManager.ItemsCount() == 0)
        {
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogFirstItem));
        }
        inventoryManager.AddItem(ItemToGive);
        //itemIsGiven = true;
    }
}
