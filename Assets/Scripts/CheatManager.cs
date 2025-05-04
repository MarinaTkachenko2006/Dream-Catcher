using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    [SerializeField] private Dialog dialog;
    [SerializeField] private Toggle speedBoostToggle;
    [SerializeField] private Toggle allItemsToggle;
    public static CheatManager Instance;
    public bool isSpeedBoosted = false;
    public bool hasAllItems = false;
    GameObject playerObject;
    private bool showCheatDialogOnExit;
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

    void Start()
    {
        PauseMenuManager.Instance.OnMenuClosed += HandleMenuClosed;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PauseMenuManager.Instance.OnMenuClosed += HandleMenuClosed;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PauseMenuManager.Instance.OnMenuClosed -= HandleMenuClosed;
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

    public void GetAllItems(bool enabled)
    {
        if (enabled)
            if (!hasAllItems)
            {
                InventoryManager.Instance.AddItem("Collar");
                InventoryManager.Instance.AddItem("Rod");
                InventoryManager.Instance.AddItem("Drawings");
                InventoryManager.Instance.AddItem("Camera");
                hasAllItems = true;
                showCheatDialogOnExit = true;
            }
    }

    private void HandleMenuClosed()
    {
        if (!showCheatDialogOnExit)
            return;

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        showCheatDialogOnExit = false;
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
            hasAllItems = false;
        }
        ResetCheatToggles();
    }

    public void ResetCheatToggles()
    {
        if (speedBoostToggle != null)
            speedBoostToggle.isOn = false;

        if (allItemsToggle != null)
            allItemsToggle.isOn = false;
    }
}
