using UnityEngine;

public enum AttackStrategyType
{
    Laser,
    Missile,
    Slow
}

[CreateAssetMenu(fileName = "NewTowerConfig", menuName = "NeonDefense/TowerConfig")]
public class TowerConfig : ScriptableObject
{
    [Header("General")]
    public string towerName = "Turret";
    public int cost = 100;
    public GameObject prefab;

    [Header("Combat")]
    public float damage = 10f;
    public float range = 5f;
    public float fireRate = 1f;
    public GameObject projectilePrefab;

    [Header("Behavior")]
    public AttackStrategyType strategyType;
}
