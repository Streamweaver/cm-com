using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetMovePosition;
    private bool isMoving = false;
    private GridPosition unitGridPosition;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private Animator unitAnimator;

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
        if (isMoving)
        {
            MoveTo();
        }
    }

    private void UpdateGridPosition()
    {
        GridPosition newGridPosition = LevelGrid.Instance.WorldPositionToGridPosition(this.transform.position);
        if (unitGridPosition != newGridPosition)
        {
            LevelGrid.Instance.RemoveUnitFromGrid(unitGridPosition, this);
            unitGridPosition = newGridPosition;
            LevelGrid.Instance.AddUnitToGrid(unitGridPosition, this);
        }
    }

    private void MoveTo()
    {
        Vector3 moveDirection = (targetMovePosition - transform.position).normalized;
        transform.position += moveDirection * Time.deltaTime * this.moveSpeed;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        float distancToMoveTarget = Vector3.Distance(transform.position, targetMovePosition);
        UpdateGridPosition();
        if (distancToMoveTarget <= stopDistance)
        {
            setMoving(false);
        }
    }
    public void HandleMoveOrder(Vector3 clickedPosition)
    {
        setMoving(true);
        targetMovePosition = clickedPosition;
    }
    private void setMoving(bool bMove)
    {
        isMoving = bMove;
        unitAnimator.SetBool("IsWalking", bMove);
    }
}
