using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private TowerConfig config;

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

        UpdateCooldown();

        if (currentTarget == null || !IsTargetValid(currentTarget))
        {
            FindTarget();
        }

        if (currentTarget != null && fireCooldown <= 0f)
        {
            Attack();
            fireCooldown = 1f / config.fireRate;
        }
    }

    private void UpdateCooldown()
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private bool IsTargetValid(Transform target)
    {
        if (target == null) return false;
        if (!target.gameObject.activeInHierarchy) return false;

        float dist = Vector3.Distance(transform.position, target.position);
        return dist <= config.range;
    }

    private void FindTarget()
    {
        // Optimization: Could use a layer mask here
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
