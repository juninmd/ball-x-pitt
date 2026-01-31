using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    public Tower CreateTower(TowerConfig config, Vector3 position)
    {
        if (config == null || config.prefab == null)
        {
             Debug.LogWarning("Invalid TowerConfig!");
             return null;
        }

        GameObject instance = Instantiate(config.prefab, position, Quaternion.identity);
        Tower tower = instance.GetComponent<Tower>();

        if (tower != null)
        {
            IAttackStrategy strategy = CreateStrategy(config.strategyType);
            tower.Initialize(config, strategy);
        }
        else
        {
            Debug.LogError("Prefab does not have a Tower component!");
        }

        return tower;
    }

    private IAttackStrategy CreateStrategy(AttackStrategyType type)
    {
        switch (type)
        {
            case AttackStrategyType.Laser: return new LaserAttackStrategy();
            case AttackStrategyType.Missile: return new MissileAttackStrategy();
            default: return new LaserAttackStrategy();
        }
    }
}
