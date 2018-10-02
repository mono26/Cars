using UnityEngine;

public static class CustomPhysics
{
    // TODO pass the angle of preference. Refactorization.
    public static float CalculateLaunchAngleForParabolicMovement(Vector3 _initialPosition, Vector3 _targetPosition, float _speed)
    {
        // angle = arctan(v2 +- Sqr(v4 - g(gx2+2yv2))/gx)
        Vector3 target = _targetPosition;
        target.y = _initialPosition.y;
        Vector3 toTarget = target - _initialPosition;
        float targetDistance = toTarget.magnitude;
        float relativeY = _targetPosition.y - _initialPosition.y;
        float vSquared = _speed * _speed;
        float gravity = Mathf.Abs(Physics.gravity.y);

        // If the distance to our target is zero, we can assume it's right on top of us (or that we're our own target).
        if (Mathf.Approximately(targetDistance, 0f))
        {
            // If we're doing a low-angle direct shot, we tweak our angle based on relative height of target.
            if (relativeY > 0) { return 90f; }
            if (relativeY < 0) { return -90f; }
        }

        float sqr = Mathf.Sqrt((vSquared * vSquared) -
                             (gravity * ((gravity * (targetDistance * targetDistance)) + (2 * relativeY * vSquared))));

        // The "underarm", parabolic arc angle
        float theta1 = Mathf.Atan((vSquared + sqr) / (gravity * targetDistance));
        // The "overarm", direct arc angle
        float theta2 = Mathf.Atan((vSquared - sqr) / (gravity * targetDistance));

        bool theta1Nan = float.IsNaN(theta1);
        bool theta2Nan = float.IsNaN(theta2);

        // If both are invalid, we early-out with a NaN to indicate no solution.
        if (theta1Nan && theta2Nan) { return float.NaN; }

        // We'll init with the parabolic arc.
        float returnTheta = theta1;

        // If the difference in heights is too small we want to use the upper arc.
        if (relativeY <= 0.5)
        {
            if (theta1Nan)
                returnTheta = theta2;
            else returnTheta = theta1;
        }
        else if (relativeY > 0.5)
        {
            if (theta2Nan)
                returnTheta = theta1;
            else returnTheta = theta2;
        }

        /*// If we want to return the direct arc
        if (arcHeight == BallisticArcHeight.UseLow)
        {
            returnTheta = theta2;
        }

        // If we want to return theta1 wherever valid, but will settle for theta2 if theta1 is invalid
        if (arcHeight == BallisticArcHeight.PreferHigh)
        {
            returnTheta = theta1Nan ? theta2 : theta1;
        }

        // If we want to return theta2 wherever valid, but will settle for theta1 if theta2 is invalid
        if (arcHeight == BallisticArcHeight.PreferLow)
        {
            returnTheta = theta2Nan ? theta1 : theta2;
        }*/

        return returnTheta * Mathf.Rad2Deg;
    }

    public static Vector3 CalculateVelocityVectorForParabolicMovement(Vector3 _initialPosition, Vector3 _targetPosition, float _speed = 10.0f)
    {
        float theta = CalculateLaunchAngleForParabolicMovement(_initialPosition, _targetPosition, _speed);
        // If our angle is impossible, we early-out.
        if (float.IsNaN(theta)) { return Vector3.zero; }

        Vector3 target = _targetPosition;
        target.y = _initialPosition.y;
        Vector3 toTarget = target - _initialPosition;
        float targetDistance = toTarget.magnitude;

        Vector3 aimVector = Vector3.forward;

        if (targetDistance > 0f)
        {
            // Normalize vector so we can rotate it.
            aimVector = toTarget / targetDistance;
            aimVector.y = 0;
        }

        Vector3 rotAxis = Vector3.Cross(aimVector, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(theta, rotAxis);
        aimVector = rotation * aimVector.normalized;

        return aimVector * _speed;
    }
}
