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

    public void HandleUnitSelection(Collider unitCollider)
    {
        if (Instance.selectedUnit == null || unitCollider.gameObject != Instance.selectedUnit.gameObject)
        {
            Instance.selectedUnit = unitCollider.GetComponent<Unit>();
            Instance.OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void HandleUnitMoveOrder(Vector3 position)
    {
        if (Instance.selectedUnit != null)
        {
            Instance.selectedUnit.HandleMoveOrder(position);
        }
    }

    public Unit GetSelectedUnit()
    {
        return Instance.selectedUnit;
    }
}
