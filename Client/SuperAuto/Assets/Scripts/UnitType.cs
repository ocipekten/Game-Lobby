using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Type")]
public class UnitType : ScriptableObject
{
    public int attack;
    public int health;
    public Sprite sprite;
    public string text;
}
