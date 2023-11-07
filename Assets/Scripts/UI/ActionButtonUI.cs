using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction action;

    private void Awake()
    {
        // Null checks
        if (!textMeshPro) Debug.LogError("TextMeshProUGUI not set in " + gameObject.name);
        if (!button) Debug.LogError("Button not set in " + gameObject.name);
        if (!selectedGameObject) Debug.LogError("SelectedGameObject not set in " + gameObject.name);

        button.onClick.AddListener(OnActionButtonClicked);
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        action = baseAction;
        textMeshPro.text = action.Label().ToUpper();
    }

    public BaseAction GetBaseAction()
    {
        return action;
    }

    public void OnActionButtonClicked()
    {
        if (UnitActionSystem.Instance.GetSelectedAction() == action)
        {
            // If the selected action is the same, clear it
            UnitActionSystem.Instance.ClearSelectedAction();
        }
        else
        {
            // Otherwise, set the action
            UnitActionSystem.Instance.SetSelectedAction(action);
        }
        UpdateSelectedVisual();
    }

    public void UpdateSelectedVisual()
    {
        bool isSelected = GetBaseAction() == UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(isSelected);
    }

    public void SetButtonInteractable(bool enableInteraction)
    {
        button.interactable = enableInteraction;
    }
}
