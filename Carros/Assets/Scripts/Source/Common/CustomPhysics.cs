// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public enum BallisticArcPreference { DirectArc, ParabolicArc}

public struct ParabolicMovementData
{
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float speed;
    private BallisticArcPreference arcPreference;

    public Vector3 GetInitialPosition { get { return initialPosition; } }
    public Vector3 GetTargetPosition { get { return targetPosition; } }
    public float GetSpeed { get { return speed; } }
    public BallisticArcPreference GetArcPreference { get { return arcPreference; } }

    public ParabolicMovementData(Vector3 _initialPosition, Vector3 _targetPositon, float _speed, BallisticArcPreference _arcPreference = BallisticArcPreference.ParabolicArc)
    {
        initialPosition = _initialPosition;
        targetPosition = _targetPositon;
        speed = _speed;
        arcPreference = _arcPreference;
        return;
    }
}

public static class CustomPhysics
{
    public static float CalculateLaunchAngleForParabolicMovement(ParabolicMovementData _parabolicData)
    {
        // angle = arctan(v2 +- Sqr(v4 - g(gx2+2yv2))/gx)
        Vector3 target = _parabolicData.GetTargetPosition;
        target.y = _parabolicData.GetInitialPosition.y;
        Vector3 vectorToTarget = target - _parabolicData.GetInitialPosition;
        float targetDistance = vectorToTarget.magnitude;
        float relativeY = _parabolicData.GetTargetPosition.y - _parabolicData.GetInitialPosition.y;
        float vSquared = _parabolicData.GetSpeed * _parabolicData.GetSpeed;
        float gravity = Mathf.Abs(Physics.gravity.y);
        // If the distance to our target is zero, we can assume it's right on top of us (or that we're our own target).
        if (Mathf.Approximately(targetDistance, 0f))
        {
            // If we're doing a low-angle direct shot, we tweak our angle based on relative height of target.
            if (relativeY > 0) { return 90f; }
            if (relativeY < 0) { return -90f; }
        }
        float sqr = Mathf.Sqrt((vSquared * vSquared) - (gravity * ((gravity * (targetDistance * targetDistance)) + (2 * relativeY * vSquared))));
        float parabolicAngle = Mathf.Atan((vSquared + sqr) / (gravity * targetDistance));
        float directAngle = Mathf.Atan((vSquared - sqr) / (gravity * targetDistance));
        float angleToReturn = PickAngle(parabolicAngle, directAngle, _parabolicData.GetArcPreference);
        return angleToReturn;
    }

    private static float PickAngle(float _angle1, float _angle2, BallisticArcPreference _arcPreference = BallisticArcPreference.ParabolicArc)
    {
        bool isTheta1Nan = float.IsNaN(_angle1);
        bool isTheta2Nan = float.IsNaN(_angle2);
        float returnTheta = 0;
        if (!isTheta1Nan && !isTheta2Nan)
        {
            // If the difference in heights is too small we want to use the upper arc.
            if (_arcPreference == BallisticArcPreference.ParabolicArc)
            {
                if (isTheta1Nan) {
                    returnTheta = _angle2;
                }
                else {
                    returnTheta = _angle1;
                }
            }
            else if (_arcPreference == BallisticArcPreference.DirectArc)
            {
                if (isTheta2Nan) {
                    returnTheta = _angle1;
                }
                else {
                    returnTheta = _angle2;
                }
            }

        }
        else {
            returnTheta = float.NaN;
        }
        return returnTheta * Mathf.Rad2Deg;
    }

    public static Vector3 CalculateVelocityVectorForParabolicMovement(ParabolicMovementData _parabolicData)
    {
        float theta = CalculateLaunchAngleForParabolicMovement(_parabolicData);
        Vector3 velocityVector = Vector3.zero;
        // If our angle is impossible, we early-out.
        if (!float.IsNaN(theta))
        {
            Vector3 target = _parabolicData.GetTargetPosition;
            target.y = _parabolicData.GetInitialPosition.y;
            Vector3 toTarget = target - _parabolicData.GetInitialPosition;
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
            velocityVector = aimVector * _parabolicData.GetSpeed;
        }
        return velocityVector;
    }
}
