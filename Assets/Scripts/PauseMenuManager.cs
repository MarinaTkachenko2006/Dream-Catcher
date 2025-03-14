using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;
    public GameObject cheatMenuUI;
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Awake()
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

    // без этого сломается чит-меню
    void Start()
    {
        if (cheatMenuUI != null)
        {
            cheatMenuUI.SetActive(false); // Скрываем чит-меню при запуске
        }
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (pauseMenuUI == null)
            pauseMenuUI = GameObject.Find("PauseMenuCanvas");

        if (scene.name == "MainMenu" || scene.name == "HubScene") // в хабе и меню паузы нет
        {
            if (pauseMenuUI != null)
                pauseMenuUI.SetActive(false);

            Time.timeScale = 1f;
            isPaused = false;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene != "MainMenu") // нет паузы в главном меню
            {
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadHub()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // чтобы, будучи на хабе, меню паузы клозилось
        if (currentScene == "Hub")
        {
            ResumeGame();
            return;
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Hub");
    }

    public void OpenCheatMenu()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        if (cheatMenuUI != null)
            cheatMenuUI.SetActive(true);
    }

    public void CloseCheatMenu()
    {
        if (cheatMenuUI != null)
            cheatMenuUI.SetActive(false);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    public void ToggleSpeedBoost(bool enabled)
{
    if (CheatManager.Instance != null)
    {
        CheatManager.Instance.ToggleSpeedBoost(enabled);
    }
}

    public void ExitGame()
    {
        Application.Quit();
    }
}
