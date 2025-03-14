using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeCheatToggle : MonoBehaviour
{
    [SerializeField] CheatManager cheatManager;

    public void GiveBike(bool toggleValue)
    {
        if (toggleValue)
        {
            cheatManager.ToggleSpeedBoost(true);
        }
        else
        {
            cheatManager.ToggleSpeedBoost(false);
        }
    }
}
