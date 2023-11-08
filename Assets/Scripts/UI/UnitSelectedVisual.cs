using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    private Unit unit;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        unit = transform.parent.gameObject.GetComponent<Unit>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (unit == null)
        {
            Debug.LogError("Unable to get unit from parent object.");
        }
        if (meshRenderer == null)
        {
            Debug.LogError("Unable to get mesh renderer.");
        }
    }

    private void OnEnable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectUnitChange;
        UpdateVisual();
    }

    void OnDestory()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectUnitChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UnitActionSystem_OnSelectUnitChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        } 
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectUnitChange;
    }
}
