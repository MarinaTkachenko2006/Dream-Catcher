using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum EnemyActionType { BasicAttack, StrongAttack, Heal, Defend, BuffAttack }
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
        playerUnit.currentMP -= 10;
        audioManager.PlaySFX(audioManager.click);
        playerUnit.Heal(30);
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
        playerUnit.restoreMana(35);
        state = BattleState.ENEMYTURN;

        playerHUD.SetHP(playerUnit.currentHP);
        playerHUD.SetMP(playerUnit.currentMP);
        dialogueText.text = "Вы привели рассудок в порядок";
        audioManager.PlaySFX(audioManager.heal);

        yield return new WaitForSeconds(2f);

        StartCoroutine(EnemyTurn());
    }


    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " готовится к действию...";
        yield return new WaitForSeconds(1f);

        EnemyAction selectedAction = ChooseEnemyAction();

        switch (selectedAction.actionType)
        {
            case EnemyActionType.BasicAttack:
                yield return StartCoroutine(EnemyBasicAttack(selectedAction));
                break;

            case EnemyActionType.StrongAttack:
                yield return StartCoroutine(EnemyStrongAttack(selectedAction));
                break;

            case EnemyActionType.Heal:
                yield return StartCoroutine(EnemyHeal(selectedAction));
                break;

            case EnemyActionType.Defend:
                yield return StartCoroutine(EnemyDefend(selectedAction));
                break;

            case EnemyActionType.BuffAttack:
                yield return StartCoroutine(EnemyBuffAttack(selectedAction));
                break;
        }
        enemyUnit.ProcessBuffs();

        if (playerUnit.currentHP <= 0)
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

    private EnemyAction ChooseEnemyAction()
    {
        List<EnemyAction> weightedActions = new List<EnemyAction>();
        foreach (var action in enemyUnit.possibleActions)
        {
            for (int i = 0; i < action.chance; i++)
            {
                weightedActions.Add(action);
            }
        }
        int randomIndex = Random.Range(0, weightedActions.Count);
        return weightedActions[randomIndex];
    }

    void PlayerTurn()
    {
        dialogueText.text = "Выберите действие:";
    }

    IEnumerator ShowManaWarning()
    {
        string originalText = dialogueText.text;
        dialogueText.text = "Недостаточно маны!";
        yield return new WaitForSeconds(1.5f);
        dialogueText.text = originalText;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (playerUnit.currentMP < 10)
        {
            StartCoroutine(ShowManaWarning());
            return;
        }

        StartCoroutine(PlayerAttack());
    }
    public void OnStrongAttackButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;

        if (playerUnit.currentMP < 15)
        {
            StartCoroutine(ShowManaWarning());
            return;
        }

        StartCoroutine(PlayerStrongAttack());
    }
    public void OnHealButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.currentMP < 10)
        {
            StartCoroutine(ShowManaWarning());
            return;
        }
        StartCoroutine(PlayerHeal());
    }

    public void OnRestoreMPButton()
    {
        audioManager.PlaySFX(audioManager.click);
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerRestoreMana());
    }

    IEnumerator EnemyBasicAttack(EnemyAction action)
    {
        int damage = Random.Range(action.minDamage, action.maxDamage + 1) + enemyUnit.attackBuff;
        dialogueText.text = enemyUnit.unitName + " " + action.description;
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(damage);
        playerHUD.SetHP(playerUnit.currentHP);
        audioManager.PlaySFX(audioManager.attack);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyStrongAttack(EnemyAction action)
    {
        int damage = Random.Range(action.minDamage, action.maxDamage + 1) + enemyUnit.attackBuff;
        dialogueText.text = enemyUnit.unitName + " " + action.description;
        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(Mathf.RoundToInt(damage * 1.5f) - enemyUnit.currentDefense);
        playerHUD.SetHP(playerUnit.currentHP);
        // audioManager.PlaySFX(audioManager.strongAttack);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyHeal(EnemyAction action)
    {
        dialogueText.text = enemyUnit.unitName + " " + action.description;
        enemyUnit.Heal(action.healAmount);
        enemyHUD.SetHP(enemyUnit.currentHP);
        audioManager.PlaySFX(audioManager.heal);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyDefend(EnemyAction action)
    {
        dialogueText.text = enemyUnit.unitName + " " + action.description;
        enemyUnit.ApplyDefenseBuff(action.healAmount, action.buffDuration);
        // audioManager.PlaySFX(audioManager.shield);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator EnemyBuffAttack(EnemyAction action)
    {
        dialogueText.text = enemyUnit.unitName + " " + action.description;
        enemyUnit.ApplyAttackBuff(action.healAmount, action.buffDuration);
        // audioManager.PlaySFX(audioManager.powerUp);

        yield return new WaitForSeconds(1f);
    }

}
