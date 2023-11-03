using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
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
            actionButtonUIList.Clear();
            foreach (BaseAction baseAction in unit.GetBaseActions())
            {
                Transform actionButtonTransfornm = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransfornm.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
                actionButtonUIList.Add(actionButtonUI);
            }
            UpdateSelectedActionVisual();
        }
    }

    private void UnitActionSystem_OnSelectedChanged(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
    }

    private void UpdateSelectedActionVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();  
        }
    }
}
