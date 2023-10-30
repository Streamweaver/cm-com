using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                if (((1 << hit.collider.gameObject.layer) & unitLayer) != 0) {
                    HandleUnitClick(hit.collider);
                }
                if (((1 << hit.collider.gameObject.layer) & environmentLayer) != 0)
                {
                    HandleEnvironmentalClick(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleUnitSpin();
        }
    }

    private void HandleUnitClick(Collider unitCollider)
    {
        if(unitCollider != null && unitCollider.TryGetComponent<Unit>(out Unit unit))
        {
            unitActionSystem.HandleUnitSelection(unit);
        }

    }

    private void HandleEnvironmentalClick(Vector3 point)
    {
        GridPosition gridPosition = LevelGrid.Instance.WorldPositionToGridPosition(point);
        unitActionSystem.HandleUnitMoveOrder(gridPosition);
    }

    private void HandleUnitSpin()
    {
        unitActionSystem.HandleUnitSpinOrder();
    }
}

