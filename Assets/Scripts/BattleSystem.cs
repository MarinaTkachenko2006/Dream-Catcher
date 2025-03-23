using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform enemyBattleStation;
    public Transform playerBattleStation;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    BattleUnit playerUnit;
    EnemyBattleUnit enemyUnit;

    public TextMeshProUGUI dialogueText;
    public BattleState state;
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(BattleLoader.Instance.GetPlayerPrefab(), playerBattleStation);
        playerUnit = playerGO.GetComponent<BattleUnit>();

        GameObject enemyGO = Instantiate(BattleLoader.Instance.GetEnemyPrefab(), enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<EnemyBattleUnit>();

        dialogueText.text = enemyUnit.introText;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        if (isDead)
        {
            state = BattleState.WON;
            enemyHUD.SetHP(enemyUnit.currentHP = 0);
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            enemyHUD.SetHP(enemyUnit.currentHP);
            dialogueText.text = "Вы нанесли " + playerUnit.damage + " урона...";

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }

    }
    // логика поведения врага в битве
    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " атакует!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Вы победили!";
            StartCoroutine(ExitBattle(true));
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Вы проиграли.";
            StartCoroutine(ExitBattle(false));
        }
    }

    IEnumerator ExitBattle(bool won)
    {
        yield return new WaitForSeconds(2f);
        if (!won)
        {
            yield return new WaitForSeconds(1f);
            LevelLoader.Instance.LoadLevel("Hub");
        }
        // GameController.Instance.SetState(GameState.FreeRoam);
        if (won)
        {
            // enemyUnit.isDefeated = false;
            BattleLoader.Instance.MarkEnemyAsDefeated(enemyPrefab.name);
            BattleLoader.Instance.loadingFromBattle = true;
            BattleLoader.Instance.returnPlayerBackToScene();
        }
        GameController.Instance.SetState(GameState.FreeRoam);
    }

    void PlayerTurn()
    {
        dialogueText.text = "Выберите действие:";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);
        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "Вы восстановили силы";

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());
    }

}
