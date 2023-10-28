using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition unitGridPosition;
    private MoveAction moveAction; 

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
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
}
