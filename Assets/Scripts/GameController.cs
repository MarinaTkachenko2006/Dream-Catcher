using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle, BattleDialog }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public static GameController Instance { get; private set; }
    GameState state;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };
        TryFindPlayerController();
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (playerController == null)
                TryFindPlayerController();

            playerController?.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
    // public GameObject GetPlayerPrefab()
    // {
    //     return playerPrefab;
    // }
    public void SetState(GameState newState)
    {
        state = newState;
    }
    private void TryFindPlayerController()
    {
        if (state == GameState.Battle) return; // в бою не надо

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();

            if (playerController == null)
            {

            }
            else
            {
                Debug.Log("PlayerController найден");
            }
        }
    }
}
