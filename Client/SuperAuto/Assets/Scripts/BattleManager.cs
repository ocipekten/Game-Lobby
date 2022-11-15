using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    TeamManager teamManager;

    private void Awake()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    private void Start()
    {
        Unit[] units = teamManager.units;

        for (int i = 0; i < 5; i++)
        {
            if (units[i] != null)
                Spawner.NewBattleUnit(units[i], new Vector3(-10f + (1.5f * i), 0f, 0f));
        }
    }
}
