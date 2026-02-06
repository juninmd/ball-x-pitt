using NUnit.Framework;
using UnityEngine;
using NeonDefense.Managers;
using NeonDefense.Core;
using NeonDefense.Enemies;
using System.Reflection;

public class PlayerHealthManagerTests
{
    private GameObject managerGO;
    private PlayerHealthManager manager;

    [SetUp]
    public void SetUp()
    {
        managerGO = new GameObject("PlayerHealthManager");
        manager = managerGO.AddComponent<PlayerHealthManager>();

        GameEvents.OnEnemyReachedGoal = null;
        GameEvents.OnGameOver = null;
    }

    [TearDown]
    public void TearDown()
    {
        if (managerGO != null)
        {
            Object.DestroyImmediate(managerGO);
        }
        GameEvents.OnEnemyReachedGoal = null;
        GameEvents.OnGameOver = null;
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        // Set initial health
        FieldInfo currentHealthField = typeof(PlayerHealthManager).GetProperty("CurrentHealth").GetSetMethod(true);
        // CurrentHealth has private set. PropertyInfo.SetValue works?
        // Wait, CurrentHealth property is: public int CurrentHealth { get; private set; }
        // Reflection on backing field is safer usually, or use setter info.
        // Backing field usually <CurrentHealth>k__BackingField
        // Or just invoke Awake?
        // Awake sets CurrentHealth = startingBits (wait, startingHealth).
        // startingHealth is serialized field.

        // Let's set startingHealth via reflection then call Awake.
        FieldInfo startingHealthField = typeof(PlayerHealthManager).GetField("startingHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        startingHealthField.SetValue(manager, 100);

        MethodInfo awakeMethod = typeof(PlayerHealthManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        awakeMethod.Invoke(manager, null);

        manager.TakeDamage(10);

        Assert.AreEqual(90, manager.CurrentHealth);
    }

    [Test]
    public void OnEnemyReachedGoal_InflictsDamage()
    {
        // Setup
        FieldInfo startingHealthField = typeof(PlayerHealthManager).GetField("startingHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        startingHealthField.SetValue(manager, 100);
        MethodInfo awakeMethod = typeof(PlayerHealthManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        awakeMethod.Invoke(manager, null);

        MethodInfo onEnableMethod = typeof(PlayerHealthManager).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
        onEnableMethod.Invoke(manager, null);

        // Trigger event
        var enemyGO = new GameObject("Enemy");
        var enemy = enemyGO.AddComponent<Enemy>();
        GameEvents.OnEnemyReachedGoal?.Invoke(enemy, 20);

        Assert.AreEqual(80, manager.CurrentHealth);

        Object.DestroyImmediate(enemyGO);
    }

    [Test]
    public void GameOver_EventTriggered_WhenHealthZero()
    {
        FieldInfo startingHealthField = typeof(PlayerHealthManager).GetField("startingHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        startingHealthField.SetValue(manager, 10);
        MethodInfo awakeMethod = typeof(PlayerHealthManager).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        awakeMethod.Invoke(manager, null);

        bool gameOverTriggered = false;
        GameEvents.OnGameOver += () => gameOverTriggered = true;

        manager.TakeDamage(10);

        Assert.AreEqual(0, manager.CurrentHealth);
        Assert.IsTrue(gameOverTriggered);
    }
}
