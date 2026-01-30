using UnityEngine;

public enum TowerType
{
    Laser,
    Missile
}

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 5f;
    public float fireRate = 1f;
    public TowerType type;

    private float fireCooldown = 0f;
    private Transform currentTarget;
    private IAttackStrategy attackStrategy;

    private void Start()
    {
        InitializeStrategy();
    }

    private void InitializeStrategy()
    {
        switch (type)
        {
            case TowerType.Laser:
                attackStrategy = new LaserAttackStrategy();
                break;
            case TowerType.Missile:
                attackStrategy = new MissileAttackStrategy();
                break;
        }
    }

    private void Update()
    {
        FindTarget();

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0 && currentTarget != null)
        {
            Attack();
            fireCooldown = 1f / fireRate;
        }
    }

    private void FindTarget()
    {
        // Placeholder for target finding logic
        // In a real implementation, this would iterate over active enemies
        // from a manager or use Physics.OverlapSphere
    }

    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            currentTarget = null;
            return;
        }

        if (Vector3.Distance(transform.position, target.position) <= range)
        {
            currentTarget = target;
        }
        else
        {
            currentTarget = null;
        }
    }

    private void Attack()
    {
        if (attackStrategy != null && currentTarget != null)
        {
            attackStrategy.Attack(currentTarget, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
