using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDoor : MonoBehaviour, Interactable
{
    [SerializeField] Dialog NotEnoughItemsDialog;
    [SerializeField] Dialog EnoughItemsDialog;
    [SerializeField] string sceneToLoad;
    InventoryManager inventoryManager = InventoryManager.Instance;
    private bool isEndingAvailable = false;

    public void Interact()
    {
        // Проверяем количество предметов в инвентаре
        if (inventoryManager.ItemsCount() < 4)
        {
            StartCoroutine(ShowDialogAndWait(NotEnoughItemsDialog));
        }
        else
        {
            StartCoroutine(ShowDialogAndWait(EnoughItemsDialog, true));
        }
    }

    private IEnumerator ShowDialogAndWait(Dialog dialog, bool isEnding = false)
    {
        // Показываем диалог
        yield return DialogManager.Instance.ShowDialog(dialog);

        // Если это концовочный диалог
        if (isEnding)
        {
            // Ждем пока игрок нажмет E после закрытия диалога
            isEndingAvailable = true;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            LevelLoader.Instance.LoadLevel(sceneToLoad);
        }
    }

    void Update()
    {
        // Обработка ввода E после закрытия диалога
        if (isEndingAvailable && Input.GetKeyDown(KeyCode.E))
        {
            isEndingAvailable = false;
        }
    }
}
