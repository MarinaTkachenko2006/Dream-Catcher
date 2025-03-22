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
        Debug.Log("НАЧАТЬ БИТВУ!!!!!!");
        BattleLoader.Instance.StartBattle(enemyPrefab);
    }

    // IEnumerator StartBattleAfterDialog(Dialog preBattleDialog, GameObject enemyPrefab)
    // {
    //     yield return StartCoroutine(DialogManager.Instance.ShowDialog(preBattleDialog));
    //     BattleLoader.Instance.StartBattle(enemyPrefab);
    // }

}
