using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            GetComponent<Collider2D>().enabled = false;
            LevelLoader.Instance.LoadLevelFast(sceneToLoad);
        }
    }
}
