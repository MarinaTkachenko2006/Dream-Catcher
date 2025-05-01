using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;
    public bool isSpeedBoosted = false;
    public bool hasAllItems = false;
    GameObject playerObject;
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
            return;
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
        playerObject = GameObject.FindWithTag("Player");
        ApplyCheats();
    }

    public void ToggleSpeedBoost(bool enabled)
    {
        isSpeedBoosted = enabled;

        if (playerObject != null)
        {
            PlayerController player = playerObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ToggleSpeedBoost(enabled);
                Debug.Log("Чит на скорость " + (enabled ? "включен" : "выключен"));
            }

        }
        else
        {
            Debug.LogError("Игрок не найден");
        }
    }

    public void GetAllItems()
    {
        InventoryManager.Instance.AddItem("Collar");
        InventoryManager.Instance.AddItem("Rod");
        InventoryManager.Instance.AddItem("?");
        InventoryManager.Instance.AddItem("Camera");
        hasAllItems = true;
    }

    // метод применяет все включенные читы при смене сцены
    void ApplyCheats()
    {
        if (isSpeedBoosted)
        {
            ToggleSpeedBoost(isSpeedBoosted);
        }
    }

    public void Reset()
    {
        if (isSpeedBoosted)
        {
            ToggleSpeedBoost(false);
        }
    }
}
