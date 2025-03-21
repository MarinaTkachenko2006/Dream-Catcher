using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public string unitName;
    // public int unitLevel;
    public int damage;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentMP -= 7;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void restoreMana(int amount)
    {
        currentMP += amount;
        if (currentMP > maxMP)
            currentMP = maxMP;
    }
}
