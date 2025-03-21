using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLoader : MonoBehaviour
{
    public static BattleLoader Instance { get; private set; }
    //[SerializeField] LevelLoader levelLoader;
    [SerializeField] GameObject playerPrefab;
    private GameObject enemyPrefab;
    // private GameObject playerPrefab;

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

    public void StartBattle(GameObject enemy)
    {
        LevelLoader loader = FindObjectOfType<LevelLoader>();
        enemyPrefab = enemy;
        // playerPrefab = gameController.GetPlayerPrefab();

        // gameController.SetState(GameState.Battle);
        LevelLoader.Instance.LoadLevel("BattleScene");
    }

    public GameObject GetEnemyPrefab() => enemyPrefab;
    public GameObject GetPlayerPrefab() => playerPrefab;
}
