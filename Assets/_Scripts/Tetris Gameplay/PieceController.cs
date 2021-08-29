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
    [SerializeField] private Vector3 rotationOffset;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private float targetAngle;
    private float rotationVelocity;
    private float fallTimer;
    private float movementCooldownTimer;
    private int invalidFallCounter;

    private void Start()
    {
        targetPosition = transform.position;
        targetAngle = GetCurrentAngle();
    }

    private void Update()
    {
        UpdateHorizontalMovement();
        UpdateFallingMovement();
        UpdateRotation();
        MovePieceTowardsTarget();
        UpdateFinalPiecePlacement();
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
            {
                invalidFallCounter = 0;
                targetPosition = newPos;
            }
            else
            {
                invalidFallCounter++;
            }
        }
    }

    private void UpdateRotation()
    {
        if (input.Rotate)
        {
            targetAngle += 90;
            float delta = Mathf.DeltaAngle(GetCurrentAngle(), targetAngle);
            transform.RotateAround(transform.TransformPoint(rotationOffset), new Vector3(0, 0, 1), delta);
            if (!IsPositionValid(targetPosition))
            {
                targetAngle -= 90;
            }
            transform.RotateAround(transform.TransformPoint(rotationOffset), new Vector3(0, 0, 1), -delta);
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
        float currentAngle = GetCurrentAngle();
        float requiredAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotationVelocity, movementSmoothTime);
        transform.RotateAround(transform.TransformPoint(rotationOffset), new Vector3(0, 0, 1), Mathf.DeltaAngle(currentAngle, requiredAngle));
    }

    private float GetCurrentAngle()
    {
        return transform.rotation.eulerAngles.z;
    }

    private void UpdateFinalPiecePlacement()
    {
        if (invalidFallCounter >= 2)
        {
            transform.position = targetPosition;
            transform.RotateAround(transform.TransformPoint(rotationOffset), new Vector3(0, 0, 1), Mathf.DeltaAngle(GetCurrentAngle(), targetAngle));
            while (transform.childCount > 0)
                GameGrid.instance.AddCubeToGrid(transform.GetChild(0));
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(rotationOffset), 0.1f);
    }
}
