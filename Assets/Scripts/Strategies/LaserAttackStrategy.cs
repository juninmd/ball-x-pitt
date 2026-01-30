using UnityEngine;

public class LaserAttackStrategy : IAttackStrategy
{
    public void ExecuteAttack(Transform origin, Transform target, TowerConfig config)
    {
        // Instant hit logic
        if (target != null)
        {
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(config.damage);
            }

            // Visuals could be handled here (e.g. enabling a LineRenderer component on the tower)
            Debug.Log($"Laser fired at {target.name} for {config.damage} damage");
        }
    }
}
