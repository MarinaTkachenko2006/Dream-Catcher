using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiverController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog1;
    [SerializeField] Dialog dialog2;
    [SerializeField] private ItemSO itemToGive;
    public bool itemIsGiven = false;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Interact()
    {
        if (!itemIsGiven)
        {
            StartCoroutine(InteractRoutine());
        }
        else
        {
            Debug.Log("Предмет уже получен.");
        }
    }

    IEnumerator InteractRoutine()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog1));
        audioManager.PlaySFX(audioManager.item);

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager не найден!");
            yield break;
        }

        InventoryManager.Instance.AddItem(itemToGive);
        
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateInventoryUI();
        }

        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog2));
        itemIsGiven = true;
    }
}
