using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingChallenger : MonoBehaviour, Interactable
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Dialog preBattleDialog;
    [SerializeField] Dialog postBattleDialog;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float destroyDelay = 0.2f;

    private bool isDefeated = false;
    private Collider2D interactionCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        interactionCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (isDefeated) return;

        if (BattleLoader.Instance.IsEnemyDefeated(enemyPrefab.name))
        {
            StartCoroutine(DisappearRoutine());
        }
        else
        {
            StartCoroutine(StartBattleRoutine());
        }
    }

    private IEnumerator StartBattleRoutine()
    {
        interactionCollider.enabled = false;
        yield return DialogManager.Instance.ShowDialog(preBattleDialog);
        string enemyName = enemyPrefab.name;

        BattleLoader.Instance.StartBattle(enemyPrefab);
        GameController.Instance.SetState(GameState.Battle);

        while (GameController.Instance.state == GameState.Battle)
        {
            yield return null;
        }

        if (BattleLoader.Instance.IsEnemyDefeated(enemyName))
        {
            yield return DisappearRoutine();
        }
        else
        {
            interactionCollider.enabled = true;
        }
    }

     private IEnumerator DisappearRoutine()
    {
        isDefeated = true;
        if (postBattleDialog.Lines.Count > 0)
        {
            yield return DialogManager.Instance.ShowDialog(postBattleDialog);
        }
        yield return StartCoroutine(FadeOut());

        Destroy(gameObject);
    }

    private IEnumerator FadeOut()
    {
        if (interactionCollider != null)
            interactionCollider.enabled = false;

        float startAlpha = spriteRenderer.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / fadeDuration);
            Color newColor = spriteRenderer.color;
            newColor.a = Mathf.Lerp(startAlpha, 0f, progress);
            spriteRenderer.color = newColor;

            yield return null;
        }
        yield return new WaitForSeconds(destroyDelay);
    }
}
