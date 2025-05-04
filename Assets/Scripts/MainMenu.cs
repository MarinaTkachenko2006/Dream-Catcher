using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    public void StartGame()
    {
        audioManager.PlaySFX(audioManager.click);
        if (GameController.Instance.gameStarts > 0)
        {
            GameController.Instance.gameStarts++;
            LevelLoader.Instance.LoadLevel("Hub");
        }

        else
        {
            GameController.Instance.gameStarts++;
            LevelLoader.Instance.LoadLevel("Tutorial");
        }
    }

    public void GoToSettingsMenu()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("SettingsMenu");
    }

    public void GoToMainMenu()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        audioManager.PlaySFX(audioManager.click);
        Application.Quit();
    }
}
