using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsController : MonoBehaviour, Interactable
{
    AudioManager audioManager;
    [SerializeField] private int LoadLevel;
    [SerializeField] private float delayBeforeLoading = 1.5f;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Interact()
    {
        audioManager.PlaySFX(audioManager.doors);

        StartCoroutine(LoadSceneWithDelay());

    }
    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoading); // Задержка перед загрузкой
        SceneManager.LoadScene(LoadLevel); // Загрузка сцены
    }
}
