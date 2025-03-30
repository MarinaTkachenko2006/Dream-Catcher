using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTeleporter : MonoBehaviour, Interactable
{
    [SerializeField] private string levelToLoad;

    public void Interact()
    {
        LevelLoader.Instance.LoadLevel(levelToLoad);
    }
}
