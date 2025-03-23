using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsController : MonoBehaviour, Interactable
{
    AudioManager audioManager;
    [SerializeField] private string levelToLoad;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Interact()
    {
        audioManager.PlaySFX(audioManager.doors);
        LevelLoader.Instance.LoadLevel(levelToLoad);
    }
}
