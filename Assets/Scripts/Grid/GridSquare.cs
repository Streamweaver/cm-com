using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;
    private List<Unit> unitList;

    public GridSquare (GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        this.unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string returnString = $"{gridPosition.X}, {gridPosition.Z}";
        foreach (Unit unit in this.unitList)
        {
            returnString += $"\n{unit}";
        }
        return returnString;
    }

    public void AddUnit(Unit unit)
    {
        this.unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        this.unitList.Remove(unit);
    }

    public List<Unit> GetUnitList() { 
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if (!HasAnyUnit())
        {
            return null;
        }
        return unitList[0];
    }
}
