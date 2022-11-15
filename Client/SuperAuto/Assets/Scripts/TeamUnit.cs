using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamUnit : Unit
{
    public int level;
    public int exp;
    public int totalExp;
    public Action OnCombine;

    public TeamUnit(Unit unit) : base(unit.type)
    {
        this.level = 1;
        this.exp = 0;
        this.totalExp = 0;
    }

    public bool Combine(Unit unit)
    {
        if (AddExp() == false) return false;
        attack = Mathf.Max(attack, unit.attack) + 1;
        health = Mathf.Max(health, unit.health) + 1;
        OnCombine.Invoke();
        return true;
    }

    public bool Combine(TeamUnit unit)
    {
        if (AddExp(unit.exp+1) == false) return false;
        attack = Mathf.Max(attack, unit.attack) + 1;
        health = Mathf.Max(health, unit.health) + 1;
        OnCombine.Invoke();
        return true;
    }

    public bool AddExp(int amount = 1)
    {
        if (totalExp + amount > 5) return false;
        else totalExp += amount;

        if (totalExp >= 5)
        {
            level = 3;
            exp = 3;
        }
        else if (totalExp >= 2)
        {
            level = 2;
            exp = totalExp - 2;
        }
        else
        {
            level = 1;
            exp = totalExp;
        }
        return true;
    }

    public string GetLevelText()
    {
        return "LVL:" + level + " EXP:" + exp + "/" + (level == 3 ? 3 : level + 1);
    }
}
