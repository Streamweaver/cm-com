using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float SpinSpeed = 360f;

    // Some stopping variables.
    private float totalSpinAmount;
    private Vector3 originalAngle;

    public void Update()
    {
        if (IsActive)
        {
            Spin();
        }
    }

    public void StartAction()
    {
        totalSpinAmount = 0f;
        originalAngle = transform.eulerAngles;
        IsActive = true;
    }

    public void Spin()
    {
        float spinAmount = SpinSpeed * Time.deltaTime;
        totalSpinAmount += spinAmount;
        transform.eulerAngles += new Vector3 (0, spinAmount, 0);
        if(totalSpinAmount >= 360)
        {
            IsActive = false;
            totalSpinAmount = 0f;
            transform.eulerAngles = originalAngle; // corrects for small drift from angle update.
        }

    }

    public void CancelAction()
    {
        IsActive = false;
    }

    public void ToggleSpin()
    {
        IsActive = !IsActive;
    }
}
