using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengerController : MonoBehaviour, Interactable
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Dialog preBattleDialog;
    [SerializeField] Dialog postBattleDialog;

    // bool dialogFinished = false;
    public void Interact()
    {
        Debug.Log($"Имя врага в контроллере: {enemyPrefab.name}");
        if (BattleLoader.Instance.IsEnemyDefeated(enemyPrefab.name))
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(postBattleDialog));
        }
        else
        {
            StartCoroutine(StartBattleAfterDialog(preBattleDialog, enemyPrefab));
        }
    }

    private IEnumerator StartBattleAfterDialog(Dialog dialog, GameObject enemyPrefab)
    {
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        yield return StartCoroutine(DialogManager.Instance.WaitForDialogToEnd());

        GameController.Instance.SetState(GameState.BattleDialog);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        Debug.Log($"Имя врага в контроллере 2: {enemyPrefab.name}");

        BattleLoader.Instance.StartBattle(enemyPrefab);
        GameController.Instance.SetState(GameState.Battle);
    }


}
