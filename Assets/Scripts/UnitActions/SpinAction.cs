using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private const float SPIN_SPEED = 360f;

    // Some stopping variables.
    private float totalSpinAmount;
    private Vector3 startAngle;

    public void Update()
    {
        if (IsActive)
        {
            Spin();
        }
    }

    public void Spin()
    {
        float spinAmount = SPIN_SPEED * Time.deltaTime;
        totalSpinAmount += spinAmount;
        transform.eulerAngles += new Vector3 (0, spinAmount, 0);
        if(totalSpinAmount >= 360)
        {
            ActionComplete();

            totalSpinAmount = 0f;
            transform.eulerAngles = startAngle; // corrects for small drift from angle update.
        }

    }

    public override bool CanTakeAction(GridPosition gridPosition)
    {
        return true;
    }

    public override void TakeAction(GridPosition gridPosition, Action callback)
    {
        ActionStart(callback);
        startAngle = transform.eulerAngles;
    }

    public override string Label()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridPosition> { unit.GetGridPosition() };
    }
}