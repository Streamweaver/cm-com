using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OnIsBusyChanged;
    private bool IsBusy = false;

    private Unit selectedUnit;
    private BaseAction selectedAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Trying to instantiate an instance of UnitActionSystem when one already exists! " + transform);
            Destroy(gameObject);
        }

    }

    public void ClearActionSystem()
    {
        ClearSelectedAction();
        ClearSelectedUnit();
        ClearBusy();
    }

    // BUSY FLAG
    private void SetBusy()
    {
        _setBusy(true);
    }

    private void ClearBusy()
    {
        _setBusy(false);
    }

    private void _setBusy(bool busy)
    {
        IsBusy = busy;
        if (!IsBusy) ClearSelectedAction();
        OnIsBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool GetIsBusy()
    {
        return IsBusy;
    }

    // UNIT OPERATIONS
    public void HandleUnitSelection(Unit unit)
    {
        if (IsBusy)
        {
            // Early return if the system is busy
            return;
        }

        // Check if the selected unit should be changed
        if (IsUnitSelectable(unit))
        {
            ChangeSelectedUnit(unit);
        }
    }

    private bool IsUnitSelectable(Unit unit)
    {
        // Determine if no unit is selected, the unit is the same, or the unit is an enemy
        return selectedUnit != unit && !unit.IsEnemy();
    }

    private void ChangeSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ClearSelectedUnit()
    {
        selectedUnit = null;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }

    // ACTION OPERATIONS
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void SetSelectedAction(BaseAction action)
    {
        GridSystemVisual.Instance.HideAllGridPositions();
        selectedAction = action;
        GridSystemVisual.Instance.ShowGridPositions(selectedAction.GetValidActionGridPositionList());
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearSelectedAction()
    {
        GridSystemVisual.Instance.HideAllGridPositions();
        selectedAction = null;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void HandleSelectedAction(GridPosition gridPosition)
    {
        if (selectedUnit.CanSpendActionPointsToTakeAction(selectedAction) && selectedAction.CanTakeAction(gridPosition))
        {
            SetBusy();
            selectedUnit.SpendActionPoints(selectedAction.GetActionPointCost());
            selectedAction.TakeAction(gridPosition, ClearBusy);
            GridSystemVisual.Instance.HideAllGridPositions();
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            ClearSelectedAction(); // If points aren't enough, make sure you clear the selected ation or it get stuck.
        }
    }
}
