using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedChanged;
        CreateUnitActionButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform t in actionButtonContainerTransform)
        {
            Destroy(t.gameObject);  // TODO refactor this to be more object like if course doesn't.
        }
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        if (unit != null)
        {
            foreach (BaseAction baseAction in unit.GetBaseActions())
            {
                Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            }
        }
    }

    private void UnitActionSystem_OnSelectedChanged(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
    }
}
