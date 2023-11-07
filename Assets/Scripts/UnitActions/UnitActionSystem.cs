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
    private bool IsBusy  = false;

    private Unit selectedUnit;
    private BaseAction selectedAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("Trying to instantiate an instance of UnitActionSystem when one already exists! " + transform);
            Destroy(gameObject);
        }

    }

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
        if(!IsBusy) ClearSelectedAction();
        OnIsBusyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool GetIsBusy()
    {
        return IsBusy;
    }

    public void HandleUnitSelection(Unit unit)
    {
        if (IsBusy) return;
        if (Instance.selectedUnit == null || unit.gameObject != Instance.selectedUnit.gameObject)
        {
            Instance.selectedUnit = unit.GetComponent<Unit>();
            Instance.OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void SetSelectedAction(BaseAction action)
    {
        GridSystemVisual.Instance.HideAllGridPositions();
        selectedAction = action;
        GridSystemVisual.Instance.ShowGridPositions(Instance.selectedAction.GetValidActionGridPositionList());
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ClearSelectedAction()
    {
        GridSystemVisual.Instance.HideAllGridPositions();
        Instance.selectedAction = null;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty );
    }

    public void HandleSelectedAction(GridPosition gridPosition)
    {
        if (selectedUnit.CanSpendActionPointsToTakeAction(selectedAction))
        {
            SetBusy();
            selectedUnit.SpendActionPoints(selectedAction.GetActionPointCost());
            selectedAction.TakeAction(gridPosition, ClearBusy);
            GridSystemVisual.Instance.HideAllGridPositions();
            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        } else
        {
            ClearSelectedAction(); // If points aren't enough, make sure you clear the selected ation or it get stuck.
        }
    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }
}
