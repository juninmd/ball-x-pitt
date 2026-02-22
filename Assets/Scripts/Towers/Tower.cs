// NeonDefense - Tower System
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting logic and delegates attack execution to an IAttackStrategy.
    /// Follows the Strategy Pattern to allow dynamic attack behaviors (Laser, Missile, etc.).
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The configuration scriptable object defining stats and strategy.")]
        [SerializeField] private TowerConfig config;

        [Tooltip("The transform point from which projectiles/attacks originate.")]
        [SerializeField] private Transform firePoint;

        [Tooltip("LayerMask to filter enemies during targeting.")]
        [SerializeField] private LayerMask enemyLayer;

        private IAttackStrategy attackStrategy;
        private float fireCountdown = 0f;
        private Enemy currentTarget;

        // Pre-allocated buffer for OverlapSphereNonAlloc to avoid GC allocations during Update
        private readonly Collider[] hitBuffer = new Collider[20];

        /// <summary>
        /// Initializes the tower with a specific configuration and strategy.
        /// Useful for Factory creation or runtime upgrades.
        /// </summary>
        /// <param name="config">The tower configuration.</param>
        /// <param name="strategy">The specific attack strategy implementation.</param>
        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
            this.fireCountdown = 0f;
        }

        private void Start()
        {
            // Fallback: If placed in editor without Factory, try to self-initialize based on config
            if (config != null && attackStrategy == null)
            {
                InitializeStrategyFromConfig();
            }

            if (firePoint == null)
            {
                firePoint = transform;
            }
        }

        /// <summary>
        /// Creates the appropriate strategy based on the TowerConfig enum.
        /// Acts as a local factory if the Strategy wasn't injected.
        /// </summary>
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
                // Extend with more cases as needed
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

        /// <summary>
        /// Finds the nearest enemy within range using non-allocating physics overlap.
        /// </summary>
        private void UpdateTarget()
        {
            // Efficiency: Search for enemies within range using a non-allocating Physics call
            // Clears previous buffer content implicitly by overwriting with new count
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);

            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                Collider hit = hitBuffer[i];
                if (hit == null) continue;

                // Optimization: Check for component.
                // Using TryGetComponent avoids garbage allocation in newer Unity versions compared to GetComponent
                if (hit.TryGetComponent<Enemy>(out var enemyComponent))
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemyComponent;
                    }
                }
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
