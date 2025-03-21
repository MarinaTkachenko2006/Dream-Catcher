using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengerController : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] Dialog preBattleDialog;
    [SerializeField] Dialog postBattleDialog;
    bool isDefeated = false;
    public void Interact()
    {
        if (isDefeated)
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(postBattleDialog));
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(preBattleDialog));
            Debug.Log("НАЧАТЬ БИТВУ!!!!!!");
            BattleLoader.Instance.StartBattle(enemyPrefab);
        }

    }

    // IEnumerator StartBattleAfterDialog(Dialog preBattleDialog, GameObject enemyPrefab)
    // {
    //     yield return StartCoroutine(DialogManager.Instance.ShowDialog(preBattleDialog));
    //     BattleLoader.Instance.StartBattle(enemyPrefab);
    // }

}
