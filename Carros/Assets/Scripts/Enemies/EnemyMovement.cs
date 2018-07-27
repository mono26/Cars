using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : HorizontalAndVerticalMovement
{
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
}
