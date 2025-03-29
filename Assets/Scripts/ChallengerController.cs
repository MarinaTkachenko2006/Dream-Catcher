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
        // Ждем, пока полностью завершится диалог (игрок сам нажал E для закрытия)
        yield return StartCoroutine(DialogManager.Instance.ShowDialog(dialog));

        // После закрытия диалога сразу запускаем битву
        BattleLoader.Instance.StartBattle(enemyPrefab);
        GameController.Instance.SetState(GameState.Battle);
    }


}
