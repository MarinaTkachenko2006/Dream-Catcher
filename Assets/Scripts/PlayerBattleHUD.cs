using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerBattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Slider mpSlider;

    public void SetHUD(BattleUnit unit)
    {
        nameText.text = unit.unitName;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        mpSlider.maxValue = unit.maxMP;
        mpSlider.value = unit.currentMP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
    public void SetMP(int mp)
    {
        mpSlider.value = mp;
    }
}
