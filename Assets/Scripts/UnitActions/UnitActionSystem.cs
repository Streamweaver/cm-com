using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
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
        IsBusy = true;
    }

    private void ClearBusy() 
    { 
        IsBusy = false;
        ShowValidGridPositions();
    }

    public void HandleUnitSelection(Unit unit)
    {
        if (IsBusy) return;
        if (Instance.selectedUnit == null || unit.gameObject != Instance.selectedUnit.gameObject)
        {
            Instance.selectedUnit = unit.GetComponent<Unit>();
            SetSelectedAction(unit.GetMoveAction());
            Instance.OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
            ShowValidGridPositions();
        }
    }

    private void ShowValidGridPositions()
    {
        if (selectedUnit != null)
        {
            GridSystemVisual.Instance.HideAllGridPositions();
            List<GridPosition> validGridPositions = selectedUnit.GetMoveAction().GetValidActionGridPositionList();
            GridSystemVisual.Instance.ShowGridPositions(validGridPositions);
        }
    }

    public void HandleUnitMoveOrder(GridPosition gridPosition)
    {
        if (IsBusy) return;
        if (Instance.selectedUnit != null)
        {
            SetBusy();
            GridSystemVisual.Instance.HideAllGridPositions();
            Instance.selectedUnit.GetMoveAction().TakeAction(gridPosition, ClearBusy);
        }
    }

    public void HandleUnitSpinOrder()
    {
        if (IsBusy) return;
        if (Instance.selectedUnit != null) {
            SetBusy();
            Instance.selectedUnit.GetSpinAction().TakeAction(new GridPosition(0,0), ClearBusy);
        }
    }

    private void HandleSelectedAction(BaseAction action)
    {

    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }
}
