using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner
{
    public static UnitBehaviour NewUnit(UnitType type, Vector3 position)
    {
        Unit unit = new Unit(type);

        GameObject unitObject = new GameObject("Unit", typeof(UnitBehaviour));
        unitObject.GetComponent<UnitBehaviour>().SetParameters(unit, position, NewDescriptionBox(unitObject), NewStatsBox(unitObject));

        return unitObject.GetComponent<UnitBehaviour>();
    }

    public static TeamUnitBehaviour NewTeamUnit(Unit unit, Vector3 position, TeamSlot slot)
    {
        TeamUnit teamUnit = new TeamUnit(unit);

        GameObject teamUnitObject = new GameObject("Team Unit", typeof(TeamUnitBehaviour));
        teamUnitObject.GetComponent<TeamUnitBehaviour>().SetParameters(teamUnit, position, NewDescriptionBox(teamUnitObject), NewStatsBox(teamUnitObject), NewLevelBox(teamUnitObject), slot);

        return teamUnitObject.GetComponent<TeamUnitBehaviour>();
    }

    public static TeamUnitBehaviour NewBattleUnit(Unit unit, Vector3 position)
    {
        TeamUnit teamUnit = new TeamUnit(unit);

        GameObject teamUnitObject = new GameObject("Team Unit", typeof(TeamUnitBehaviour));
        teamUnitObject.GetComponent<TeamUnitBehaviour>().SetParameters(teamUnit, position, NewDescriptionBox(teamUnitObject), NewStatsBox(teamUnitObject));

        return teamUnitObject.GetComponent<TeamUnitBehaviour>();
    }

    public static GameObject NewDescriptionBox(GameObject parent)
    {
        GameObject descriptionBox = new GameObject("Description Box");
        descriptionBox.transform.parent = parent.transform;
        descriptionBox.AddComponent<SpriteRenderer>().sprite = Constants.descriptionBoxBackground;
        descriptionBox.transform.position = parent.transform.position + Vector3.up;
        descriptionBox.transform.localScale = new Vector3(1f, 1f, 1f);
        GameObject descriptionBoxText = new GameObject("Description Box Text");
        descriptionBoxText.transform.parent = descriptionBox.transform;
        descriptionBoxText.transform.position = descriptionBox.transform.position;
        TextMeshPro textMesh = descriptionBoxText.AddComponent<TextMeshPro>();
        descriptionBoxText.transform.localScale = Vector3.one;
        textMesh.color = Color.black;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.fontSize = 1.5f;

        return descriptionBox;
    }

    public static GameObject NewStatsBox(GameObject parent)
    {
        GameObject statsUI = new GameObject("StatsUI");
        statsUI.transform.parent = parent.transform;
        statsUI.transform.position = parent.transform.position + Vector3.down / 2f;
        TextMeshPro statsText = statsUI.AddComponent<TextMeshPro>();
        statsText.transform.localScale = Vector3.one;
        statsText.fontSize = 5f;
        statsText.alignment = TextAlignmentOptions.Center;

        return statsUI;
    }

    public static GameObject NewLevelBox(GameObject parent)
    {
        GameObject levelUI = new GameObject("LevelUI");
        levelUI.transform.parent = parent.transform;
        levelUI.transform.position = parent.transform.position + Vector3.up / 2f;
        TextMeshPro levelText = levelUI.AddComponent<TextMeshPro>();
        levelText.transform.localScale = Vector3.one;
        levelText.fontSize = 2f;
        levelText.alignment = TextAlignmentOptions.Center;

        return levelUI;
    }

    public static GameObject SpawnError(string error)
    {
        GameObject gameObject = new GameObject("Error");
        gameObject.transform.parent = GameObject.FindObjectOfType<Canvas>().transform;
        gameObject.transform.position = Vector3.zero;

        TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
        textMeshPro.fontSize = 4.5f;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.text = error;
        textMeshPro.color = Color.red;

        GameObject.Destroy(gameObject, 2f);
        return gameObject;
    }
}
