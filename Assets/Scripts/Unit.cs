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
    private int actionPoints { get; set; }
    private int _actionPointMax = 2;

    private void OnDestroy()
    {
        LevelGrid.Instance.RemoveUnitFromGrid(unitGridPosition, this);
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

    public void ResetActionPoints()
    {
        actionPoints = _actionPointMax;
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        Debug.Log($"Action {baseAction} of {baseAction.GetActionPointCost()} with {actionPoints} left");
        return baseAction.GetActionPointCost() <= actionPoints;
    }

    public void SpendActionPoints(int actionPointCost)
    {
        actionPoints -= actionPointCost;
    }
}
