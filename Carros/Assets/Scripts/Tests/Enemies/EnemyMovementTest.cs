using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using NUnit.Framework;

public class EnemyMovementTest
{
    [Test]
    public void CreatesTestEnemyMovement()
    {
        EnemyMovement testEnemyMovement = TestsHelperMethods.CreateScriptInstanceInGameObject<EnemyMovement>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testEnemyMovement);
        return;
    }

    /*
    [Test]
    public void InitializeTestEnemyMovement()
    {
        EnemyMovement testEnemyMovement = TestHelperMethods.CreateTestScriptInstanceInGameObject<EnemyMovement>("InitializeTestEnemyMovement");
        testEnemyMovement.gameObject.AddComponent<NavMeshAgent>();
        ReflectionBehaviour testReflection = testEnemyMovement.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testEnemyMovement.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void CalculatesPathWhenDestinationIsGiven()
    {
        EnemyMovement testEnemyMovement = TestHelperMethods.CreateTestScriptInstanceInGameObject<EnemyMovement>("CalculatesPathWhenDestinationIsGiven");
        testEnemyMovement.gameObject.AddComponent<NavMeshAgent>();
        TestHelperMethods.InitializeTestGameObject(testEnemyMovement.gameObject);
        Vector3 testDestination = new Vector3(10, 0, 0);
        testEnemyMovement.SetNavigationDestination(testDestination);
        Assert.NotNull(testEnemyMovement.GetNavigationComponent.path);
    }

    [Test]
    public void NavigationComponentDisabledWhenNavigationIsDeactivated()
    {
        EnemyMovement testEnemyMovement = TestHelperMethods.CreateTestScriptInstanceInGameObject<EnemyMovement>("NavigationComponentDisabledWhenNavigationIsDeactivated");
        testEnemyMovement.gameObject.AddComponent<NavMeshAgent>();
        TestHelperMethods.InitializeTestGameObject(testEnemyMovement.gameObject);
        testEnemyMovement.ActivateNavMeshNavigation(false);
        Assert.Equals(testEnemyMovement.GetNavigationComponent.enabled, false);
    }

    [Test]
    public void NavigationComponentEnabledWhenNavigationIsActivated()
    {
        EnemyMovement testEnemyMovement = TestHelperMethods.CreateTestScriptInstanceInGameObject<EnemyMovement>("NavigationComponentEnabledWhenNavigationIsActivated");
        testEnemyMovement.gameObject.AddComponent<NavMeshAgent>();
        TestHelperMethods.InitializeTestGameObject(testEnemyMovement.gameObject);
        testEnemyMovement.ActivateNavMeshNavigation(true);
        Assert.Equals(testEnemyMovement.GetNavigationComponent.enabled, true);
    }
    */
}
