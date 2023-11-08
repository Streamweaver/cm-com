using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    private UnitActionSystem unitActionSystem;

    // Set these in the inspector.
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask environmentLayer;

    private void Awake()
    {
        unitActionSystem = GetComponent<UnitActionSystem>();
    }

    private void Update()
    {
        if (UnitActionSystem.Instance.GetIsBusy() || !TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (TryRaycast(out hit, GetCurrentLayerMask()))
            {
                HandleClick(hit);
            }
        }
    }

    private bool TryRaycast(out RaycastHit hit, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, float.MaxValue, layerMask);
    }

    private void HandleClick(RaycastHit hit)
    {
        if (UnitActionSystem.Instance.GetSelectedAction() == null)
        {
            HandleUnitClick(hit);
        }
        else
        {
            HandleAction(hit);
        }
    }

    private LayerMask GetCurrentLayerMask()
    {
        if (UnitActionSystem.Instance.GetSelectedAction() == null)
        {
            return unitLayer;
        }
        else
        {
            return environmentLayer;
        }
    }

    private void HandleAction(RaycastHit hit)
    {
        GridPosition gridPosition = LevelGrid.Instance.WorldPositionToGridPosition(hit.point);
        UnitActionSystem.Instance.HandleSelectedAction(gridPosition);
    }

    private void HandleUnitClick(RaycastHit hit)
    {
        Collider unitCollider = hit.collider;
        if (unitCollider != null && unitCollider.TryGetComponent<Unit>(out Unit unit))
        {
            unitActionSystem.HandleUnitSelection(unit);
        }
    }
}
