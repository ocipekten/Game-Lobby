using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamUnitBehaviour : UnitBehaviour
{
    public TeamUnit teamUnit;

    protected GameObject levelBox;
    protected TextMeshPro levelText;
    public TeamSlot slot;

    public void SetParameters(TeamUnit unit, Vector3 pos, GameObject descriptionBox, GameObject statsBox, GameObject levelBox, TeamSlot slot)
    {
        this.unit = unit;
        this.teamUnit = unit;
        this.pos = pos;
        this.descriptionBox = descriptionBox;
        this.statsBox = statsBox;
        this.levelBox = levelBox;
        this.slot = slot;
        teamUnit.OnCombine += OnCombine;
    }

    public void SetParameters(TeamUnit unit, Vector3 pos, GameObject descriptionBox, GameObject statsBox)
    {
        this.unit = unit;
        this.teamUnit = unit;
        transform.position = pos;
        this.descriptionBox = descriptionBox;
        this.statsBox = statsBox;
    }

    protected override void Start()
    {
        base.Start();
        if (levelBox != null) 
        {
            levelText = levelBox.GetComponent<TextMeshPro>();
            RefreshLevelBox();
        }
        SetSlot(slot);
        ChangePos(pos);
        StartCoroutine(Wiggle(.9f, 1.1f, .1f, -.5f));
    }

    public void OnSell()
    {
        FindObjectOfType<TeamManager>().changeGold(teamUnit.level);
        Delete();
    }

    protected override void OnRelease()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Team Slot");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            TeamSlot teamSlot = hit.collider.GetComponent<TeamSlot>();
            if (teamSlot != null) teamSlot.OnDrop(this);
            else OnSell();
        }
    }

    public void SetSlot(TeamSlot slot)
    {
        if (this.slot != null && slot.teamUnit == null) this.slot.FreeSlot();
        this.slot = slot;
        this.slot.teamUnit = this;        
        ChangePos(this.slot.transform.position);
    }

    public void RefreshLevelBox()
    {
        levelText.SetText(((TeamUnit)unit).GetLevelText());
    }

    public void OnCombine()
    {
        RefreshLevelBox();
        RefreshStatsBox();
        StartCoroutine(Wiggle(.9f, 1.1f, .1f, -.5f));
    }

    public void ToggleLevel(bool x)
    {
        levelBox.SetActive(x);
    }
}
