using UnityEngine;

public class MissileAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform origin, Transform target, TowerConfig config)
    {
        if (config.projectilePrefab == null)
        {
            Debug.LogWarning("MissileAttackStrategy: No projectile prefab in config!");
            return;
        }

        // Use pooling
        var projectile = ProjectilePool.Instance.Get();
        projectile.transform.position = origin.position;
        projectile.Initialize(target);

        // Set properties from config
        // Note: speed could also be in config, but for now we set damage.
        projectile.damage = config.damage;
    }
}
