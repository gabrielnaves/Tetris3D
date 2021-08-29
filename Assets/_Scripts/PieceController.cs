using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    [SerializeField] private GameplayInputData input;
    [SerializeField] private float movementAmount;
    [SerializeField] private float movementSmoothTime;
    [SerializeField] private float movementCooldownTime;
    [SerializeField] private float fallTime;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float fallTimer;
    private float movementCooldownTimer;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateFallingMovement();
        MovePieceTowardsTarget();
    }

    private void UpdateHorizontalMovement()
    {
        movementCooldownTimer += Time.deltaTime;
        if (input.MovementDown != 0 || (input.Movement != 0 && movementCooldownTimer >= movementCooldownTime))
        {
            movementCooldownTimer = 0;
            var direction = input.Movement > 0 ? Vector3.right : Vector3.left;
            var newPos = targetPosition + direction * movementAmount;
            if (IsPositionValid(newPos))
                targetPosition = newPos;
        }
    }

    private void UpdateFallingMovement()
    {
        fallTimer += Time.deltaTime;
        float time = input.Fall ? fallTime / 3 : fallTime;
        if (fallTimer >= time)
        {
            fallTimer = 0;
            var newPos = targetPosition + Vector3.down * movementAmount;
            if (IsPositionValid(newPos))
                targetPosition = newPos;
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        if (!GameGrid.instance) return true;
        var difference = position - transform.position;
        foreach (Transform child in transform)
        {
            if (!GameGrid.instance.IsPositionValid(child.position + difference))
                return false;
        }
        return true;
    }

    private void MovePieceTowardsTarget()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, movementSmoothTime);
    }
}
