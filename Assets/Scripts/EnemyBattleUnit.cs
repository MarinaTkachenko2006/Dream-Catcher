using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleUnit : BattleUnit
{
    public string introText;
    public string location;
    public int SecretePunchDamage;
    public string SecretePunchText;

    [Header("Enemy AI Settings")]
    public List<EnemyAction> possibleActions;
    public int currentDefense = 0;
    public int attackBuff = 0;
    public int buffDuration = 0;

    public void ApplyDefenseBuff(int defenseAmount, int duration)
    {
        currentDefense += defenseAmount;
        buffDuration = duration;
    }

    public void ApplyAttackBuff(int attackAmount, int duration)
    {
        attackBuff += attackAmount;
        buffDuration = duration;
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public void ProcessBuffs()
    {
        if(buffDuration > 0)
        {
            buffDuration--;
            if(buffDuration <= 0)
            {
                currentDefense = 0;
                attackBuff = 0;
            }
        }
    }
}
