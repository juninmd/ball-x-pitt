using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting logic and delegates attack execution to an IAttackStrategy.
    /// Follows the Strategy Pattern.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private TowerConfig config;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private IAttackStrategy attackStrategy;
        private float fireCountdown = 0f;
        private Enemy currentTarget;

        // Pre-allocated buffer for OverlapSphereNonAlloc to avoid GC
        private readonly Collider[] hitBuffer = new Collider[20];

        /// <summary>
        /// Initializes the tower with a specific configuration and strategy.
        /// Useful for Factory creation.
        /// </summary>
        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
        }

        private void Start()
        {
            // If placed in editor without Factory, try to self-initialize based on config
            if (config != null && attackStrategy == null)
            {
                InitializeStrategyFromConfig();
            }
        }

        private void InitializeStrategyFromConfig()
        {
            switch (config.strategyType)
            {
                case AttackStrategyType.Laser:
                    attackStrategy = new LaserAttackStrategy();
                    break;
                case AttackStrategyType.Missile:
                    attackStrategy = new MissileAttackStrategy();
                    break;
                default:
                    Debug.LogWarning($"Unknown strategy type: {config.strategyType}. Defaulting to Laser.");
                    attackStrategy = new LaserAttackStrategy();
                    break;
            }
        }

        private void Update()
        {
            if (config == null) return;

            UpdateTarget();

            if (currentTarget != null)
            {
                if (fireCountdown <= 0f)
                {
                    Attack();
                    fireCountdown = 1f / config.fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;
        }

        private void UpdateTarget()
        {
            // Efficiency: Search for enemies within range using a non-allocating Physics call
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);

            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                Collider hit = hitBuffer[i];
                if (hit == null) continue;

                // Optimization: Check for component
                if (hit.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemyComponent;
                    }
                }

                // Clear reference to help GC (though buffer is reused, it's good practice)
                hitBuffer[i] = null;
            }

            // Update target if valid and within range
            if (nearestEnemy != null && shortestDistance <= config.range)
            {
                currentTarget = nearestEnemy;
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
                attackStrategy.Attack(currentTarget, firePoint, config);
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
}
