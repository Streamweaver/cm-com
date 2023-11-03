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
        selectedAction = null;
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
    }

    public void ClearSelectedAction()
    {
        GridSystemVisual.Instance.HideAllGridPositions();
        Instance.selectedAction = null;
    }

    public void HandleSelectedAction(GridPosition gridPosition)
    {
        selectedAction.TakeAction(gridPosition, ClearBusy);
        GridSystemVisual.Instance.HideAllGridPositions();
    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }
}
