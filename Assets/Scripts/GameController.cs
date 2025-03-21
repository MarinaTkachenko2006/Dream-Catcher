using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    // [SerializeField] private GameObject playerPrefab;
    public static GameController Instance { get; private set; }
    GameState state;

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
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
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
}
