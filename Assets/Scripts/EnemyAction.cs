using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAction
{
    public EnemyActionType actionType;
    [Range(0, 100)] public int chance = 50;
    public int minDamage;
    public int maxDamage;
    public int healAmount;
    public int buffDuration;
    public string description;
}
