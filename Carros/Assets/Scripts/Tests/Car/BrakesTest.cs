using UnityEngine;
using NUnit.Framework;

public class BrakesTest
{
    [Test]
    public void CreatesTestBrakes()
    {
        Brakes testBrakes = TestHelperMethods.CreateTestScriptInstanceInGameObject<Brakes>("CreatesTestBrakes");
        Assert.NotNull(testBrakes);
        return;
    }

    [Test]
    public void InitializeTestBrakes()
    {
        Brakes testBrakes = TestHelperMethods.CreateTestScriptInstanceInGameObject<Brakes>("InitializeTestBrakes");
        ReflectionBehaviour testReflection = testBrakes.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testBrakes.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void GetRealFootBrakeTorqueValue()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("GetRealFootBrakeTorqueValue");
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(0f);
        Assert.NotNull(testFootBrakeTorque);
        return;
    }

    [Test]
    public void GetRealHandBrakeTorqueValue()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("GetRealHandBrakeTorqueValue");
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(0f);
        Assert.NotNull(testHandBrakeTorque);
        return;
    }

    [Test]
    public void ZeroFootBrakeTorqueWhenNoInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("ZeroFootBrakeTorqueWhenNoInputApplied");
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(0);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void ZeroHandBrakeTorqueWhenNoInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("ZeroHandBrakeTorqueWhenNoInputApplied");
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(0);
        Assert.IsTrue(testHandBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void GreaterThanZeroFootBrakeTorqueWhenInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("GreaterThanZeroFootBrakeTorqueWhenInputApplied");
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(1);
        Assert.Greater(testFootBrakeTorque, 0);
        return;
    }

    [Test]
    public void GreaterThanZeroHandBrakeTorqueWhenInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("GreaterThanZeroHandBrakeTorqueWhenInputApplied");
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(1);
        Assert.Greater(testHandBrakeTorque, 0);
        return;
    }

    [Test]
    public void NeverNegativeFootBrakeTorqueWhenNegativeInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("NeverNegativeFootBrakeTorqueWhenNegativeInputApplied");
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(-1);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void NeverNegativeHandBrakeTorqueWhenNegativeInputApplied()
    {
        Brakes testBrakes = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>("NeverNegativeHandBrakeTorqueWhenNegativeInputApplied");
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(-1);
        Assert.IsTrue(testHandBrakeTorque.Equals(0));
        return;
    }
}
