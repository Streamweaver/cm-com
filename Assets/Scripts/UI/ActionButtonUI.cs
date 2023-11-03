using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;

    private BaseAction action;

    // private bool IsActiveAction;  // TODO: Use this to set an active visual on the button.

    private void Awake()
    {
        action = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        //action = null;
        // IsActiveAction = false;
        button.onClick.AddListener(OnActionButtonClicked);
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        action = baseAction;
        Debug.Log($"Button action set for {baseAction}");
        textMeshPro.text = action.Label().ToUpper();
    }

    public void OnActionButtonClicked()
    {
        if( action == null ) {
            Debug.Log("Action is NULL");
        }

        Debug.Log("Button Clicked!");
        if (UnitActionSystem.Instance.GetSelectedAction() == action)
        {
            Debug.Log($"Actions {action} and {UnitActionSystem.Instance.GetSelectedAction()}  are the same, clearing!");
            // If the selected action is the same, clear it
            UnitActionSystem.Instance.ClearSelectedAction();
        }
        else
        {
            Debug.Log($"Setting action to {action}");
            // Otherwise, set the action
            UnitActionSystem.Instance.SetSelectedAction(action);
        }
    }
}
