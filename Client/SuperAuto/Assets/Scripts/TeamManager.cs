using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    public List<UnitType> type;

    public List<UnitBehaviour> spawnedUnits;

    public List<TeamSlot> teamSlots;

    public Unit[] units;

    public int numberOfSpawns;

    public TextMeshProUGUI goldText;

    public static int gold;

    void Start()
    {
        spawnedUnits = new List<UnitBehaviour>();
        gold = 1;
        Roll();
        changeGold(10);
    }

    public TeamSlot GetNextSlot(TeamSlot slot)
    {
        int x = teamSlots.IndexOf(slot);
        return null;        
    }

    public void PrintSlots()
    {
        string x = "";
        foreach(TeamSlot slot in teamSlots)
        {
            if (slot.teamUnit != null)
                x += slot.teamUnit.teamUnit.type.name + " ";
            else x += " - ";
        }
        Debug.Log(x);
    }

    public bool changeGold(int x)
    {
        if (gold + x >= 0)
        {
            gold += x;
            RefreshGoldBox();
            return true;
        }
        
        return false;
    }

    void RefreshGoldBox()
    {
        goldText.SetText("Gold: " + gold);
    }

    public void Roll()
    {
        if (!changeGold(-1)) return;

        for (int i = 0; i < spawnedUnits.Count; i++)
        {
            if (spawnedUnits[i] == null) continue;
            spawnedUnits[i].Delete();
        }

        spawnedUnits.Clear();

        for (int i = 0; i < numberOfSpawns; i++)
        {
            UnitType randomType = type[Random.Range(0, type.Count)];
            Vector3 spawnPos = transform.position + (i * Vector3.right * 1.5f);
            spawnedUnits.Add(Spawner.NewUnit(randomType, spawnPos));
        }        
    }

    public void Fight()
    {
        units = new Unit[5];
        for (int i = 0; i < 5; i++)
        {
            if (teamSlots[i].teamUnit != null)
                units[i] = teamSlots[i].teamUnit.teamUnit;
            else
                units[i] = null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Single);
    }
}

public static class Constants
{
    public static Sprite descriptionBoxBackground = Resources.Load<Sprite>("Square");
    //public static Font descriptionBoxFont = Resources.GetBuiltinResource<TMPro.font>("Arial.ttf");
}
