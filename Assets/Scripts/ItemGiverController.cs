using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiverController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog1;
    [SerializeField] Dialog dialog2;
    public string ItemToGive;
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
    }

    IEnumerator InteractRoutine()
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog1));
        audioManager.PlaySFX(audioManager.item);
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog2));
        itemIsGiven = true;
    }
}
