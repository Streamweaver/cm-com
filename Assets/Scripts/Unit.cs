using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition unitGridPosition;
    private MoveAction moveAction;  // Caching this object.

    public Animator unitAnimator;

    private void Awake()
    {
        if(unitAnimator == null)
        {
            Debug.LogError("Unit Animator not set in inspector!");
        }
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

    public MoveAction GetMoveAction()
    {
        if (!moveAction)
        {
            if (!TryGetComponent<MoveAction>(out moveAction)) // This should haved the effect of caching this call.
            {
                Debug.LogError("Unable to get Move Action on Unit!");
            }
        }
        return moveAction;
    }
}
