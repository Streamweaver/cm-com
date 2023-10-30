using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    // public event 

    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    
    [SerializeField] private Unit selectedUnit;

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

    public void HandleUnitSelection(Unit unit)
    {
        if (Instance.selectedUnit == null || unit.gameObject != Instance.selectedUnit.gameObject)
        {
            Instance.selectedUnit = unit.GetComponent<Unit>();
            Instance.OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
            GridSystemVisual.Instance.HideAllGridPositions();
            List<GridPosition> validGridPositions = unit.GetMoveAction().GetValidActionGridPositionList();
            GridSystemVisual.Instance.ShowGridPositions(validGridPositions);
        }
    }

    public void HandleUnitMoveOrder(GridPosition gridPosition)
    {
        if (Instance.selectedUnit != null)
        {
            Instance.selectedUnit.GetMoveAction().HandleMoveOrder(gridPosition);
        }
    }

    public void HandleUnitSpinToggle()
    {
        if (Instance.selectedUnit != null) {
            Instance.selectedUnit.GetSpinAction().ToggleSpin();
        }
    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }
}
