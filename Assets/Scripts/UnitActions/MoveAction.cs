using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MoveAction : BaseAction
{
    private Vector3 targetMovePosition;

    private int maxMoveDistance = 2;

    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float stopDistance = 0.1f;

    private void Start()
    {
        actionPointCost = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 moveDirection = (targetMovePosition - transform.position).normalized;
        transform.position += moveDirection * Time.deltaTime * this.moveSpeed;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        float distancToMoveTarget = Vector3.Distance(transform.position, targetMovePosition);
        if (distancToMoveTarget <= stopDistance)
        {
            StopMoving();
            ActionComplete();
        }
    }

    public override bool CanTakeAction(GridPosition gridPosition)
    {
        return IsValidActionGridPosition(gridPosition);
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        if (!CanTakeAction(gridPosition))
        {
            callback.Invoke();
            return;
        }
        ActionStart(callback);
        targetMovePosition = LevelGrid.Instance.GridPositionToWorldPosition(gridPosition);
        StartMoving();
    }
    private void setMoving(bool bMove)
    {
        unitAnimator.SetBool("IsWalking", bMove);
    }

    private void StartMoving()
    {
        setMoving(true);
    }

    private void StopMoving()
    {
        setMoving(false);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition targetGridPosition = offsetGridPosition + unitGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(targetGridPosition)) {  continue; }
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(targetGridPosition)) { continue; } 
                validGridPositions.Add(targetGridPosition);
            }
        }
        return validGridPositions;
    }

    public override string Label()
    {
        return "Move";
    }
}
