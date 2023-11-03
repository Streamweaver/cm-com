using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool IsActive = false;

    public Action OnActionCompleted;

    protected virtual void Awake()
    {
        if(!TryGetComponent<Unit>(out unit))
        {
            Debug.LogError("Action is unable to get the unit component!");
        }
    }

    public abstract string Label();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public override string ToString()
    {
        return Label();
    }
}
