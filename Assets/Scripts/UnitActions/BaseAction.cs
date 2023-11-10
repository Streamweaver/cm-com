using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool IsActive = false;
    protected int actionPointCost = 1;

    public Action OnActionCompleted;

    protected virtual void Awake()
    {
        if(!TryGetComponent<Unit>(out unit))
        {
            Debug.LogError("Action is unable to get the unit component!");
        }
    }

    protected void ActionStart(Action callback)
    {
        IsActive = true;
        OnActionCompleted = callback;
    }

    protected void ActionComplete()
    {
        IsActive = false;
        OnActionCompleted?.Invoke();
        OnActionCompleted = null;
    }

    public abstract string Label();
    
    public abstract bool CanTakeAction(GridPosition gridPosition);

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost()
    {
        return actionPointCost;
    }

    public override string ToString()
    {
        return Label();
    }
}
