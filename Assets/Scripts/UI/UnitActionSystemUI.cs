using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;
    private Dictionary<Unit, List<ActionButtonUI>> unitActionButtonDictionary;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
        unitActionButtonDictionary = new Dictionary<Unit, List<ActionButtonUI>>();
    }

    private void Start()
    {
        // Subscribe to events from the UnitActionSystem
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnIsBusyChanged += UnitActionSystem_OnBusyChanged;

        CreateOrUpdateUnitActionButtons();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnIsBusyChanged -= UnitActionSystem_OnBusyChanged;
    }

    private void CreateOrUpdateUnitActionButtons()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (!selectedUnit)
        {
            return;
        }

        // Clear the action button list for the current unit
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.gameObject.SetActive(false);
        }
        actionButtonUIList.Clear();

        if (!unitActionButtonDictionary.ContainsKey(selectedUnit))
        {
            unitActionButtonDictionary.Add(selectedUnit, CreateUnitActionButtons(selectedUnit));
        }

        List<ActionButtonUI> unitActionButtons = unitActionButtonDictionary[selectedUnit];
        foreach (ActionButtonUI actionButtonUI in unitActionButtons)
        {
            actionButtonUI.gameObject.SetActive(true);
            actionButtonUIList.Add(actionButtonUI);
        }

        UpdateSelectedActionVisual();
    }

    private List<ActionButtonUI> CreateUnitActionButtons(Unit unit)
    {
        List<ActionButtonUI> localActionButtonList = new List<ActionButtonUI>();

        foreach (BaseAction baseAction in unit.GetBaseActions())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            localActionButtonList.Add(actionButtonUI);
        }

        return localActionButtonList;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateOrUpdateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        CreateOrUpdateUnitActionButtons();
    }

    private void UnitActionSystem_OnBusyChanged(object sender, EventArgs e)
    {
        bool isSystemBusy = UnitActionSystem.Instance.GetIsBusy();
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.SetButtonInteractable(!isSystemBusy);
        }
    }

    private void UpdateSelectedActionVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}
