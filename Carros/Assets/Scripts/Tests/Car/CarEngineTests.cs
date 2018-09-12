using UnityEngine;
using NUnit.Framework;

public class CarEngineTests
{
    [Test]
    public void CreatesTestCarEngine()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        CarEngine testCarEngine = testGameObject.AddComponent<CarEngine>();
        Assert.NotNull(testCarEngine);
        return;
    }

    [Test]
    public void ZeroTorqueWhenNoInputApplied()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        CarEngine testCarEngine = testGameObject.AddComponent<CarEngine>();
        TestHelperMethods.InitializeTestGameObject(testCarEngine.gameObject);
        float testTorque = testCarEngine.GetTorqueToApply(0f);
        Assert.IsTrue(testTorque.Equals(0));
        return;
    }

    [Test]
    public void EngineCapsSpeed()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        CarEngine testCarEngine = testGameObject.AddComponent<CarEngine>();
        TestHelperMethods.InitializeTestGameObject(testCarEngine.gameObject);
        Vector3 testVelocity = Vector3.forward * 999f;
        Vector3 capedTestVelocity = testCarEngine.CapVelocityMagnitudeToMaxSpeed(testVelocity);
        Assert.IsTrue(testVelocity.magnitude > capedTestVelocity.magnitude);
        return;
    }

    [Test]
    public void EngineCalculateZeroRevolutions()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        CarEngine testCarEngine = testGameObject.AddComponent<CarEngine>();
        TestHelperMethods.InitializeTestGameObject(testCarEngine.gameObject);
        float testSpeed = 0;
        testCarEngine.CalculateEngineRevolutions(testSpeed);
        Assert.IsTrue(testCarEngine.GetCurrentEngineRevolutions.Equals(0));
        return;
    }

    /*[Test]
    public void CarEngineTestsSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator CarEngineTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }*/
}
