using NUnit.Framework;
using UnityEngine;
using NeonDefense.Managers;
using NeonDefense.Core;
using NeonDefense.Enemies;
using System.Reflection;

public class EconomyManagerTests
{
    private GameObject managerGO;
    private EconomyManager manager;

    [SetUp]
    public void SetUp()
    {
        managerGO = new GameObject("EconomyManager");
        manager = managerGO.AddComponent<EconomyManager>();

        // Reset GameEvents
        GameEvents.OnEnemyKilled = null;
    }

    [TearDown]
    public void TearDown()
    {
        if (managerGO != null)
        {
            Object.DestroyImmediate(managerGO);
        }
        GameEvents.OnEnemyKilled = null;
    }

    [Test]
    public void SpendBits_ReducesAmount_WhenEnough()
    {
        // Set currentBits via reflection
        FieldInfo currentBitsField = typeof(EconomyManager).GetField("currentBits", BindingFlags.NonPublic | BindingFlags.Instance);
        currentBitsField.SetValue(manager, 100);

        bool success = manager.SpendBits(50);

        Assert.IsTrue(success);
        Assert.AreEqual(50, manager.CurrentBits);
    }

    [Test]
    public void SpendBits_Fails_WhenNotEnough()
    {
        FieldInfo currentBitsField = typeof(EconomyManager).GetField("currentBits", BindingFlags.NonPublic | BindingFlags.Instance);
        currentBitsField.SetValue(manager, 10);

        bool success = manager.SpendBits(50);

        Assert.IsFalse(success);
        Assert.AreEqual(10, manager.CurrentBits);
    }

    [Test]
    public void OnEnemyKilled_AddsBits()
    {
        // We need to simulate the event.
        // EconomyManager subscribes in OnEnable.
        // We need to manually call OnEnable or verify it was called.
        // AddComponent calls Awake and OnEnable?
        // In EditMode, maybe.
        // Let's force call OnEnable via reflection to be sure, or just public method if it was public.
        // Method is private.
        MethodInfo onEnableMethod = typeof(EconomyManager).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
        onEnableMethod.Invoke(manager, null);

        // Set initial bits
        FieldInfo currentBitsField = typeof(EconomyManager).GetField("currentBits", BindingFlags.NonPublic | BindingFlags.Instance);
        currentBitsField.SetValue(manager, 0);

        // Trigger event
        var enemyGO = new GameObject("Enemy");
        var enemy = enemyGO.AddComponent<Enemy>();
        GameEvents.OnEnemyKilled?.Invoke(enemy, 50);

        Assert.AreEqual(50, manager.CurrentBits);

        Object.DestroyImmediate(enemyGO);
    }
}
