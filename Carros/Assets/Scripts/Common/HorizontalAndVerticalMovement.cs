using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAndVerticalMovement : EntityComponent
{
    [Header("Horizontal and Vertical settings")]
    [SerializeField]
    protected float acceleration; // m / s2 (meters per second)
    [SerializeField]
    protected float horizontalFactor; // How much acceleration can be used to move horizontaly
    [SerializeField]
    protected float maxSpeed; // m / s

    protected Vector3 movementDirection;

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
        Vector3 horizontalSpeed = transform.right * _horizontalValue * acceleration * horizontalFactor;
        Vector3 verticalSpeed = transform.forward * _verticalValue * acceleration;
        entity.Body.velocity += (horizontalSpeed + verticalSpeed);

        return;
    }
}
