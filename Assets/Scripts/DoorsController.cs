using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsController : MonoBehaviour, Interactable
{
    AudioManager audioManager;
    [SerializeField] private string levelToLoad;
    private bool isTeleporting = false;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Interact()
    {
        if (isTeleporting) return;
        isTeleporting = true;
        audioManager.PlaySFX(audioManager.doors);
        LevelLoader.Instance.LoadLevel(levelToLoad);
    }
}
