using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomPhysics
{
    /// <summary>
    /// Used in projectile motion to calculate the angle needed to reach certain distance and height.
    /// </summary>
    /// <param name="_height"> Max height of the parabolic movement.</param>
    /// <param name="_distance"> Max reach of the parabolic movement.</param>
    /// <returns></returns>
    public static float CalculateLaunchAngleForParabolicMovement(float _height = 3.0f, float _distance = 3.0f)
    {
        // Equation h = Rtan(angle)/4.
        float tanAngle = (_height * 4)/_distance;
        float launchAngle = Mathf.Atan(tanAngle) * Mathf.Rad2Deg;

        return launchAngle;
    }

    /// <summary>
    /// Used in projectile motion to calculate the speed needed to reach a certain distance with an angle.
    /// </summary>
    /// <param name="_angle"> Angle to launche the parabolic movement.</param>
    /// <param name="_distance"> Max reach of the parabolic movement.</param>
    /// <returns></returns>
    public static float CalculateSpeedToPerformParabolicMovement(float _angle, float _distance, float _relativeY)
    {
        // Equation d = (Vo2/g)sin(2angle)
        float twoTheta = (2 * _angle) * Mathf.Deg2Rad;
        float speedSquare = (_distance * Mathf.Abs(Physics.gravity.y) / Mathf.Sin(twoTheta));
        float speed = Mathf.Sqrt(speedSquare); 

        if (_distance > 0)
        {
            return speed;
        }

        return 0;
    }

    public static Vector3 CalculateVelocityVectorForParabolicMovement(Vector3 _initialPosition, Vector3 _targetPosition, float _height = 3.0f)
    {
        Vector3 target = _targetPosition;
        target.y = _initialPosition.y;
        Vector3 directionToTarget = target - _initialPosition;
        float targetDistance = directionToTarget.magnitude;
        float relativeY = _initialPosition.y - _targetPosition.y;

        float angle = CalculateLaunchAngleForParabolicMovement(_height, targetDistance);
        float speed = CalculateSpeedToPerformParabolicMovement(angle, targetDistance, relativeY);

        Vector3 launchVector = Vector3.forward;

        if (targetDistance > 0f)
        {
            // Flatten aim vector so we can rotate it
            launchVector = directionToTarget / targetDistance;
            launchVector.y = 0;
        }

        Vector3 rotAxis = Vector3.Cross(launchVector, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(angle, rotAxis);
        launchVector = rotation * launchVector.normalized;
        Debug.DrawRay(_initialPosition, launchVector, Color.red);

        return launchVector * speed;
    }
}
