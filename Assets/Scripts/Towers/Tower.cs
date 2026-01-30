using UnityEngine;

public class Tower : MonoBehaviour
{
    private TowerConfig config;
    private IAttackStrategy attackStrategy;
    private Transform currentTarget;
    private float fireCooldown;

    public void Initialize(TowerConfig config, IAttackStrategy strategy)
    {
        this.config = config;
        this.attackStrategy = strategy;
        this.fireCooldown = 0f;
    }

    private void Update()
    {
        if (config == null || attackStrategy == null) return;

        FindTarget();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0 && currentTarget != null)
        {
            Attack();
            fireCooldown = 1f / config.fireRate;
        }
    }

    private void FindTarget()
    {
        // In a real optimized game, we'd query an EnemyManager for active enemies
        // instead of Physics.OverlapSphere every frame.
        // But for this scope, Physics is acceptable.

        Collider[] hits = Physics.OverlapSphere(transform.position, config.range);
        float minDist = float.MaxValue;
        Transform bestTarget = null;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var enemy))
            {
                float d = Vector3.Distance(transform.position, hit.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    bestTarget = hit.transform;
                }
            }
        }

        currentTarget = bestTarget;
    }

    private void Attack()
    {
        if (currentTarget != null)
        {
            attackStrategy.ExecuteAttack(transform, currentTarget, config);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (config != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, config.range);
        }
    }
}
