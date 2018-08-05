using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : HorizontalAndVerticalMovement
{
    public enum ForwardRotationMode { Instant, Partial}

    [Header("Enemy Movement settings")]
    [SerializeField]
    protected float angularSpeed = 45.0f;   // Radians per second
    [SerializeField]
    protected bool isFacingMovementDirection = true;
    public bool IsFacingMovementDirection { set { isFacingMovementDirection = value; } }

    [Header("Enemy Movement components")]
    protected NavMeshAgent enemyNavigation;

    /*public override void FixedFrame()
    {
        if(movementDirection.Equals(Vector3.zero) == true || isFacingMovementDirection == false) { return; }

        FaceMovementDirection(ForwardRotationMode.Instant);

        base.FixedFrame();

        Debug.Log(entity.gameObject.name + " Velocity " + entity.Body.velocity);

        return;
    }*/

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void SetDestination(Vector3 _destination)
    {
        if(enemyNavigation == null) { return; }

        enemyNavigation.SetDestination(_destination);

        return;
    }

    /// <summary>
    /// Used set the next movement direction.
    /// </summary>
    /// <param name="_direction"> The new normalized direction.</param>
    public void SetMoveDirection(Vector3 _direction)
    {
        if(_direction.magnitude > 1)
            _direction = _direction.normalized;

        movementDirection = _direction;

        return;
    }

    protected void FaceMovementDirection(ForwardRotationMode _rotationMode)
    {
        switch(_rotationMode)
        {
            case ForwardRotationMode.Instant:
                entity.Body.rotation = Quaternion.LookRotation(entity.Body.velocity.normalized);
                break;

            case ForwardRotationMode.Partial:
                float delta = Vector3.SignedAngle(entity.transform.forward, entity.Body.velocity.normalized, Vector3.up);
                Quaternion direction = Quaternion.LookRotation(entity.Body.velocity.normalized);
                //entity.Body.rotation = Quaternion.Lerp(entity.Body.rotation, direction, delta, angularSpeed);
                break;
        }

        return;
    }
}
