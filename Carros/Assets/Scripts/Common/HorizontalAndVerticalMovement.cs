using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAndVerticalMovement : EntityComponent
{
    [Header("Horizontal and Vertical settings")]
    [SerializeField]
    protected float acceleration; // m / s2 (meters per second)
    [SerializeField][Range(0,1.0f)]
    protected float horizontalFactor = 0.3f; // How much acceleration can be used to move horizontaly
    [SerializeField]
    protected float maxSpeed; // m / s

    [Header("Editor debugging")]
    [SerializeField]
    protected Vector3 movementDirection; // It must be a normalized Vector3

    public void SetMovementValues(float _acceleration, float _maxSpeed)
    {
        maxSpeed = _maxSpeed;
        acceleration = _acceleration;

        return;
    }

    public override void FixedFrame()
    {
        if (movementDirection.Equals(Vector3.zero) == true) { return; }

        ApplyVerticalAndHorizontalMovement(movementDirection.x, movementDirection.z);

        return;
    }

    protected override void HandleInput()
    {
        if (entity.Input == null) { return; }

        if (entity.Type == Entity.EntityType.Playable)
            movementDirection = entity.Input.Movement;

        return;
    }

    protected void ApplyVerticalAndHorizontalMovement(float _horizontalValue, float _verticalValue)
    {
        // TODO check for the correct way to move a rigidbody: velocity or addForce.
        Vector3 horizontalSpeed = Vector3.right * _horizontalValue * acceleration * horizontalFactor;
        Vector3 verticalSpeed = Vector3.forward * _verticalValue * acceleration;
        entity.Body.velocity += (horizontalSpeed + verticalSpeed);

        return;
    }
}
