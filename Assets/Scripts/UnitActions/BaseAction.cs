using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool IsActive = false;

    protected virtual void Awake()
    {
        if(!TryGetComponent<Unit>(out unit))
        {
            Debug.LogError("Action is unable to get the unit component!");
        }
    }
}
