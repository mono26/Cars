using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : AIEntityComponent
{
    public enum ForwardRotationMode { Instant, Partial}

    [Header("Enemy Movement settings")]
    [SerializeField]
    protected float angularSpeed = 45.0f;   // Radians per second
    [SerializeField]
    protected bool isFacingMovementDirection = true;
    public bool IsFacingMovementDirection { set { isFacingMovementDirection = value; } }

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void SetDestination(Vector3 _destination)
    {
        if(!aiEntity.Navigation) { return; }

        if(aiEntity.Navigation.isOnNavMesh)
            aiEntity.Navigation.SetDestination(_destination);

        return;
    }

    public void SetMovementValues(float _acceleration, float _maxSpeed)
    {
        if (!aiEntity.Navigation) { return; }

        aiEntity.Navigation.speed = _maxSpeed;
        aiEntity.Navigation.acceleration = _acceleration;

        return;
    }
}
