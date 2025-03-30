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

    public PlayerBattleHUD playerHUD;
    public BattleHUD enemyHUD;

    BattleUnit playerUnit;
    EnemyBattleUnit enemyUnit;

    public TextMeshProUGUI dialogueText;
    public BattleState state;
    AudioManager audioManager;
    public GameObject backgroundsContainer;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        enemyPrefab = BattleLoader.Instance.enemyPrefab;
        GameObject playerGO = Instantiate(BattleLoader.Instance.GetPlayerPrefab(), playerBattleStation);
        playerUnit = playerGO.GetComponent<BattleUnit>();

        GameObject enemyGO = Instantiate(BattleLoader.Instance.enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<EnemyBattleUnit>();

        // ActivateBackground(enemyUnit.location); // пока не надо

        dialogueText.text = enemyUnit.introText;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
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
        // ActivateBackground("");
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
    void ActivateBackground(string location)
    {
        foreach (Transform bg in backgroundsContainer.transform)
        {
            bg.gameObject.SetActive(bg.name == location);
        }
    }

    IEnumerator PlayerAttack()
    {
        playerUnit.currentMP -= 10;
        playerHUD.SetMP(playerUnit.currentMP);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        audioManager.PlaySFX(audioManager.attack);

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

    IEnumerator PlayerStrongAttack()
    {
        playerUnit.currentMP -= 10;
        playerHUD.SetMP(playerUnit.currentMP);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage * 100);
        audioManager.PlaySFX(audioManager.boom);

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
            dialogueText.text = "Вы нанесли " + playerUnit.damage + " урона";

            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerHeal()
    {
        audioManager.PlaySFX(audioManager.click);
        playerUnit.Heal(20);
        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
        playerHUD.SetMP(playerUnit.currentMP);
        dialogueText.text = "Вы восстановили силы";
        audioManager.PlaySFX(audioManager.heal);

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerRestoreMana()
    {
        audioManager.PlaySFX(audioManager.click);
        playerUnit.restoreMana(15);
        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
        playerHUD.SetMP(playerUnit.currentMP);
        dialogueText.text = "Вы привели рассудок в порядок";
        audioManager.PlaySFX(audioManager.heal);

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
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
    void PlayerTurn()
    {
        dialogueText.text = "Выберите действие:";
    }
    public void OnAttackButton()
    {
        // audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }
    public void OnStrongAttackButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerStrongAttack());
    }
    public void OnHealButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerHeal());
    }

    public void OnRestoreMPButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerRestoreMana());
    }

}
