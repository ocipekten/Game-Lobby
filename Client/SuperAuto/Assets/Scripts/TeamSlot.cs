using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSlot : MonoBehaviour
{
    public TeamUnitBehaviour teamUnit;
    private TeamManager teamManager;

    private void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
    }

    public void OnDrop(UnitBehaviour unit)
    {
        if (unit == null || teamUnit == unit) return;

        if (teamUnit == null)
        {
            InsertUnit(unit);
        }
        else
        {
            if (teamUnit.unit.type == unit.unit.type)
            {
                CombineUnits(unit);
            }
            else if (unit is TeamUnitBehaviour)
            {
                SwapUnits((TeamUnitBehaviour)unit);
            }
        }
        teamManager.PrintSlots();
    }

    public void FreeSlot()
    {
        this.teamUnit = null;
    }

    public void SwapUnits(TeamUnitBehaviour teamUnit)
    {
        this.teamUnit.SetSlot(teamUnit.slot);
        teamUnit.SetSlot(this);
    }

    public void InsertUnit(UnitBehaviour unitBehaviour)
    {
        if (unitBehaviour is TeamUnitBehaviour)
        {
            TeamUnitBehaviour newUnit = (TeamUnitBehaviour)unitBehaviour;
            newUnit.SetSlot(this);
        }
        else
        {
            if (teamManager.changeGold(-3))
            {
                this.teamUnit = Spawner.NewTeamUnit(unitBehaviour.unit, transform.position, this);
                unitBehaviour.Delete();
            }
        }
    }

    public void CombineUnits(UnitBehaviour unit)
    {
        if (unit is TeamUnitBehaviour)
        {
            if (this.teamUnit.teamUnit.Combine(((TeamUnitBehaviour)unit).teamUnit)) unit.Delete();
        }
        else 
        {
            if (((this.teamUnit.teamUnit.Combine(unit.unit)))) unit.Delete();;
        }
    }
}
