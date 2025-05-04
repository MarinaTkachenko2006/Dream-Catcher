using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Dialog[] tutorialDialogs;
    private int dialogIndex;

    private void Start()
    {
        StartCoroutine(RunTutorial());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { }
    }

    private IEnumerator RunTutorial()
    {
        while (dialogIndex < tutorialDialogs.Length)
        {
            yield return DialogManager.Instance.ShowDialog(tutorialDialogs[dialogIndex]);
            yield return null;
            dialogIndex++;
        }

        LevelLoader.Instance.LoadLevel("Hub");
    }
}
// LevelLoader.Instance.LoadLevel("Hub");