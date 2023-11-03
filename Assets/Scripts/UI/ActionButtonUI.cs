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
        if ( GetBaseAction() == UnitActionSystem.Instance.GetSelectedAction() ) {
            selectedGameObject.SetActive(true);
        }
        else
        {
            selectedGameObject.SetActive(false);
        }
    }
}
