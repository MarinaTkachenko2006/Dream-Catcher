using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int lettersPerSecond;
    public event Action OnShowDialog;
    public event Action OnHideDialog;
    public static DialogManager Instance { get; private set; }
    public bool IsDialogFinished { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Dialog dialog;
    int currentLine = 0;
    bool IsTyping;
    Coroutine typingCoroutine;

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        currentLine = 0;
        dialogBox.SetActive(true);
        typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
        yield return new WaitUntil(() => dialogBox.activeSelf == false);

        IsDialogFinished = true;
    }

    public void HandleUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.E)))
        {
            if (IsTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = dialog.Lines[currentLine];
                IsTyping = false;
            }
            else
            {
                ++currentLine;
                if (currentLine < dialog.Lines.Count)
                {
                    typingCoroutine = StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                }
                else
                {
                    dialogBox.SetActive(false);
                    currentLine = 0;
                    OnHideDialog?.Invoke();
                    IsDialogFinished = true;
                }
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        IsTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        IsTyping = false;
    }
    public IEnumerator WaitForDialogToEnd()
    {
        while (dialogBox.activeSelf)
        {
            yield return null;
        }
    }
}
