using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector3 targetMovePosition;
    private bool isMoving = false;
    private Unit unit;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private float stopDistance = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent<Unit>(out unit))
        {
            Debug.LogError("Unable to get Unit.");
        }
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
        unit.UpdateGridPosition();
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
        unit.unitAnimator.SetBool("IsWalking", bMove);
    }
}
