using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    private int turnNumber = 1;

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnNextTurn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("Trying to instantiate an instance of TurnSystem when one already exists! " + transform);
            Destroy(gameObject);
        }
    }

    public void NextTurn()
    {
        turnNumber++;

        OnNextTurn?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
