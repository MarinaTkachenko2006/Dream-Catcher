using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoader : MonoBehaviour
{
    public static BattleLoader Instance { get; private set; }
    [SerializeField] public GameObject playerPrefab;
    public Vector3 position = new Vector3();
    public string lastScene;
    public bool loadingFromBattle = false;
    private GameObject enemyPrefab;
    private HashSet<string> defeatedEnemies = new HashSet<string>();
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
        rememberPlayerPosition();
        enemyPrefab = enemy;
        // playerPrefab = gameController.GetPlayerPrefab();

        // gameController.SetState(GameState.Battle);
        LevelLoader.Instance.LoadLevel("BattleScene");
    }
    public void rememberPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        lastScene = SceneManager.GetActiveScene().name;
    }

    public void returnPlayerBackToScene()
    {
        // LevelLoader.Instance.LoadLevel(lastScene);

        // yield return new WaitUntil(() => SceneManager.GetActiveScene().name == lastScene);

        // // Ждём появления игрока
        // GameObject player = null;
        // while (player == null)
        // {
        //     yield return null; 
        //     player = GameObject.FindGameObjectWithTag("Player");
        //     Debug.Log("Пытаюсь");
        // }

        // Debug.Log("ПОЛУЧИЛОСЬ!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        // player.transform.position = position;
        LevelLoader.Instance.LoadLevel(lastScene);
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (loadingFromBattle)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = position;
            }
            else
            {
                Debug.Log("Игрок не найден после загрузки сцены!");
            }
            loadingFromBattle = false;
            // SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void MarkEnemyAsDefeated(string enemyID)
    {
        defeatedEnemies.Add(enemyID);
    }

    public bool IsEnemyDefeated(string enemyID)
    {
        return defeatedEnemies.Contains(enemyID);
    }

    public GameObject GetEnemyPrefab() => enemyPrefab;
    public GameObject GetPlayerPrefab() => playerPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
