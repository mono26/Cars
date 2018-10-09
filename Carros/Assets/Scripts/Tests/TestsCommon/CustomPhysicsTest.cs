using UnityEngine;
using NUnit.Framework;

public class CustomPhysicsTest
{
    [Test]
    public void GetUpperStraightAngleForTargetOnTop()
    {
        Vector3 testInitialPosition = Vector3.zero;
        Vector3 testTargetPositon = new Vector3(0, 1, 0);
        float testSpeed = 1.0f;
        ParabolicMovementData parabolicData = new ParabolicMovementData(testInitialPosition, testTargetPositon, testSpeed);
        float testAngle = CustomPhysics.CalculateLaunchAngleForParabolicMovement(parabolicData);
        float solutionAngle = 90;
        Assert.IsTrue(((testAngle < solutionAngle) ? (solutionAngle - testAngle) : (testAngle - solutionAngle)) <= 0.1);
    }

    [Test]
    public void GetLowerStraightAngleForTargetOnBottom()
    {
        Vector3 testInitialPosition = Vector3.zero;
        Vector3 testTargetPositon = new Vector3(0, -1, 0);
        float testSpeed = 1.0f;
        ParabolicMovementData parabolicData = new ParabolicMovementData(testInitialPosition, testTargetPositon, testSpeed);
        float testAngle = CustomPhysics.CalculateLaunchAngleForParabolicMovement(parabolicData);
        float solutionAngle = -90;
        Assert.IsTrue(((testAngle < solutionAngle) ? (solutionAngle - testAngle) : (testAngle - solutionAngle)) <= 0.1);
    }

    [Test]
    public void GetRightUpperAngleForADistantTarget()
    {
        Vector3 testInitialPosition = Vector3.zero;
        Vector3 testTargetPositon = new Vector3(3, 0, 0);
        float testSpeed = 10f;
        ParabolicMovementData parabolicData = new ParabolicMovementData(testInitialPosition, testTargetPositon, testSpeed, BallisticArcPreference.UpperArc);
        float testAngle = CustomPhysics.CalculateLaunchAngleForParabolicMovement(parabolicData);
        float solutionAngle = 81.451f;
        Assert.IsTrue(((testAngle < solutionAngle) ? (solutionAngle - testAngle) : (testAngle - solutionAngle)) <= 0.1);
    }

    [Test]
    public void GetRightLowerAngleForADistantTarget()
    {
        Vector3 testInitialPosition = Vector3.zero;
        Vector3 testTargetPositon = new Vector3(3, 0, 0);
        float testSpeed = 10f;
        ParabolicMovementData parabolicData = new ParabolicMovementData(testInitialPosition, testTargetPositon, testSpeed, BallisticArcPreference.LowerArc);
        float testAngle = CustomPhysics.CalculateLaunchAngleForParabolicMovement(parabolicData);
        float solutionAngle = 8.549f;
        Assert.IsTrue(((testAngle < solutionAngle) ? (solutionAngle - testAngle) : (testAngle - solutionAngle)) <= 0.1);
    }
}
