using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;
    private Dictionary<Unit, List<ActionButtonUI>> unitActionButtonDictionary;
    private bool IsDisplayed = false;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
        unitActionButtonDictionary = new Dictionary<Unit, List<ActionButtonUI>>();
        ValidateComponents();

    }

    private void ValidateComponents()
    {
        if (!actionButtonPrefab) Debug.LogError("[UnitActionSystemUI] ActionButtonPrefab not set in " + gameObject.name);
        if (!actionButtonContainerTransform) Debug.LogError("[UnitActionSystemUI] ActionButtonContainerTransform not set in " + gameObject.name);
        if (!actionPointsText) Debug.LogError("[UnitActionSystemUI] ActionPointsText not set in " + gameObject.name);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnUnitOrActionChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnUnitOrActionChanged;
        UnitActionSystem.Instance.OnIsBusyChanged += UnitActionSystem_OnBusyChanged;
        TurnSystem.Instance.OnNextTurn += TurnSystem_OnNextTurn;

        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
    }

    private void UnsubscribeFromEvents()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnUnitOrActionChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnUnitOrActionChanged;
        UnitActionSystem.Instance.OnIsBusyChanged -= UnitActionSystem_OnBusyChanged;
        TurnSystem.Instance.OnNextTurn -= TurnSystem_OnNextTurn;

        Unit.OnAnyActionPointChanged -= Unit_OnAnyActionPointChanged;
    }

    private void Start()
    {
        SubscribeToEvents();
        UpdateActionUI(null);
    }

    private void UpdateActionUI(Unit selectedUnit)
    {
        if (!selectedUnit)
        {
            SetDisplay(false);
            return;
        }
        SetDisplay(true);
        UpdateActionButtons(selectedUnit);
        UpdateActionPointsText(selectedUnit);
        UpdateSelectedActionVisual();
    }

    private void UpdateActionButtons(Unit unit)
    {
        if (unit == null) return;

        // Avoid unnecessary dictionary lookups
        if (!unitActionButtonDictionary.TryGetValue(unit, out var buttonsForUnit))
        {
            buttonsForUnit = CreateUnitActionButtons(unit);
            unitActionButtonDictionary[unit] = buttonsForUnit;
        }

        // Activate buttons only for the selected unit
        DeactivateActionUIButtons();
        SetActionButtonUIButtons(buttonsForUnit);

        bool isSystemBusy = UnitActionSystem.Instance.GetIsBusy();
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.SetButtonInteractable(!isSystemBusy && unit.CanSpendActionPointsToTakeAction(actionButtonUI.GetBaseAction()));
        }
    }

    private void DeactivateActionUIButtons()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.gameObject.SetActive(false);
        }
        actionButtonUIList.Clear();
    }

    private void SetActionButtonUIButtons(List<ActionButtonUI> actionButtons)
    {
        actionButtonUIList.AddRange(actionButtons);
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.gameObject.SetActive(true);
        }
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

    private void UnitActionSystem_OnUnitOrActionChanged(object sender, EventArgs e)
    {
        Unit unit = UnitActionSystem.Instance.GetSelectedUnit();
        if (!unit)
        {
            SetDisplay(false);
            return;
        }
        SetDisplay(true);
        UpdateActionUI(unit);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, EventArgs e)
    {
        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        if (!UnitActionSystem.Instance.GetSelectedUnit()) return;
        bool isSystemBusy = UnitActionSystem.Instance.GetIsBusy();
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            if (isSystemBusy)
            {
                actionButtonUI.SetButtonInteractable(false);
            }
            else
            {
                bool canTakeAction = UnitActionSystem.Instance.GetSelectedUnit().CanSpendActionPointsToTakeAction(actionButtonUI.GetBaseAction());
                actionButtonUI.SetButtonInteractable(canTakeAction);
            }

        }
    }

    private void UpdateSelectedActionVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPointsText(Unit unit)
    {
        if (!unit)
        {
            actionPointsText.text = "";
            return;
        }
        actionPointsText.text = $"Action Points: {unit.GetActionPoints()}";   
    }

    private void TurnSystem_OnNextTurn(object empty, EventArgs e)
    {
        UnitActionSystem.Instance.ClearActionSystem();
        UpdateActionUI(UnitActionSystem.Instance.GetSelectedUnit());
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText(UnitActionSystem.Instance.GetSelectedUnit());
    }

    private void SetDisplay(bool display)
    {
        if(display == IsDisplayed) return;
        IsDisplayed = display;
        actionButtonContainerTransform.gameObject.SetActive(display);
        actionPointsText.gameObject.SetActive(display);
    }
}
