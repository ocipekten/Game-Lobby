using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public UnitType type;

    public int attack;
    public int health;
    public string description;
    public Sprite sprite;

    public Unit(UnitType type)
    {
        this.type = type;
        this.attack = type.attack;
        this.health = type.health;
        this.description = type.text;
        this.sprite = type.sprite;
    }

    public string GetStatsText()
    {
        return "<color=\"red\">" + attack + "</color> <color=\"green\">" + health + "</color>";
    }
}
