using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomPhysics
{
    public static Vector3 CalculateJumpVelocityFromTimeAndAngle(Vector3 _initialPosition, Vector3 _targetPosition, float _angle = 45.0f, float _flightTime = 1.0f)
    {
        if (float.IsNaN(_angle)) { return Vector3.zero; }

        Vector3 target = _targetPosition;
        target.y = _initialPosition.y;
        Vector3 directionToTarget = target - _initialPosition;
        float targetDistance = directionToTarget.magnitude;
        float speed = (targetDistance * _flightTime) * Mathf.Cos(_angle);

        Debug.Log(speed.ToString());

        Vector3 jumpVector = Vector3.forward;

        if (targetDistance > 0f)
        {
            // Flatten aim vector so we can rotate it
            jumpVector = directionToTarget / targetDistance;
            jumpVector.y = 0;
        }

        Vector3 rotAxis = Vector3.Cross(jumpVector, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(-_angle, rotAxis);
        jumpVector = rotation * jumpVector.normalized;

        return jumpVector * speed;
    }
}
