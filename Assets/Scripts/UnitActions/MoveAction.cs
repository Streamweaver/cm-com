using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetMovePosition;
    private bool isMoving = false;
    private int maxMoveDistance = 4;
    private Unit unit;

    public Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float stopDistance = 0.1f;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        if (unitAnimator == null)
        {
            Debug.LogError("Unit Animator not set in inspector!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            MoveTo();
        }
    }

    private void MoveTo()
    {
        Vector3 moveDirection = (targetMovePosition - transform.position).normalized;
        transform.position += moveDirection * Time.deltaTime * this.moveSpeed;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        float distancToMoveTarget = Vector3.Distance(transform.position, targetMovePosition);
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

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition > gridPositions = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = offsetGridPosition + unitGridPosition;
            }
        }
        return gridPositions;
    }
}
