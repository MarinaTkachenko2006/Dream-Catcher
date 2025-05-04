using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiverController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog1;
    [SerializeField] Dialog dialog2;
    [SerializeField] private Dialog dialogFirstItem;
    [SerializeField] private Dialog dialogLastItem;

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
        else if (inventoryManager.ItemsCount() == 3)
        {
            dialogLastItem.Lines[0] = "*Вы почувствовали, что ранее запертая дверь в междусновье открылась.*";
            yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialogLastItem));
        }
        inventoryManager.AddItem(ItemToGive);
        //itemIsGiven = true;
    }
}
