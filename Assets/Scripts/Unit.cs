using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveAction))]
public class Unit : MonoBehaviour
{
    private GridPosition unitGridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints;
    private const int ACTION_POINT_MAX = 3;

    [SerializeField]
    private bool isEnemy = false;

    public static event EventHandler OnAnyActionPointChanged;

    private int Health = 100;

    private void OnDestroy()
    {
        if (LevelGrid.Instance != null)
        {
            LevelGrid.Instance.RemoveUnitFromGrid(unitGridPosition, this);
        }
        if (TurnSystem.Instance != null)
        {
            TurnSystem.Instance.OnNextTurn -= TurnSystem_OnEndTurn;
        }
    }

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
        ResetActionPoints();
    }

    private void Start()
    {
        if (LevelGrid.Instance == null)
        {
            Debug.LogError("Level Grid is not set!");
        }
        unitGridPosition = LevelGrid.Instance.WorldPositionToGridPosition(this.transform.position);
        LevelGrid.Instance.AddUnitToGrid(unitGridPosition, this);

        // LISTENERS
        TurnSystem.Instance.OnNextTurn += TurnSystem_OnEndTurn;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateGridPosition();
    }

    public void UpdateGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.WorldPositionToGridPosition(this.transform.position);
        if (unitGridPosition != newGridPosition)
        {
            LevelGrid.Instance.RemoveUnitFromGrid(unitGridPosition, this);
            unitGridPosition = newGridPosition;
            LevelGrid.Instance.AddUnitToGrid(unitGridPosition, this);
        }
    }

    public GridPosition GetGridPosition()
    {
        return unitGridPosition;
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public BaseAction[] GetBaseActions()
    {
        return baseActionArray;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return baseAction.GetActionPointCost() <= actionPoints;
    }

    private void ResetActionPoints()
    {
        actionPoints = ACTION_POINT_MAX;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SpendActionPoints(int actionPointCost)
    {
        actionPoints -= actionPointCost;
        OnAnyActionPointChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    private void TurnSystem_OnEndTurn(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            ResetActionPoints();
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public bool IsEnemyOf(Unit otherUnit)
    {
        return IsEnemy() != otherUnit.IsEnemy();
    }

    public bool ApplyDamage(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }
}
