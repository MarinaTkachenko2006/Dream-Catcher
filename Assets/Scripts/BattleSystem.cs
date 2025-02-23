using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    BattleUnit playerUnit;
    BattleUnit enemyUnit;

    public Text dialogueText;
    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, enemySpawnPoint);
        playerUnit = playerGO.GetComponent<BattleUnit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPoint);
        enemyUnit = enemyGO.GetComponent<BattleUnit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " a";
    }


}
