using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI turnNumberText;
    [SerializeField]
    private Button endTurnButton;

    private void OnValidate()
    {
        if (turnNumberText == null)
        {
            Debug.LogError($"TurnSystemUI: turnNumberText on {gameObject} is not set in the inspector.");
        }
        if (endTurnButton == null)
        {
            Debug.LogError($"TurnSystemUI: endTurnButton on {gameObject} is not set in the inspector.");
        }
    }

    private void OnDestroy()
    {
        TurnSystem.Instance.OnNextTurn -= TurnSystem_OnTurnChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnDisplay();
        endTurnButton.onClick.AddListener(OnEndTurnClicked);
        TurnSystem.Instance.OnNextTurn += TurnSystem_OnTurnChanged;
    }

    public void OnEndTurnClicked()
    {
        TurnSystem.Instance.NextTurn();
    }

    private void UpdateTurnDisplay() {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
            turnNumberText.color = Color.white;
        } else
        {
            turnNumberText.text = "Enemy Turn";
            turnNumberText.color = Color.red;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnDisplay();
    }
}
