using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting logic and delegates attack behavior to a strategy.
    /// Uses Object Pooling for projectiles (via strategy) and optimization for target searching.
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

        [Header("Debug Info")]
        [SerializeField] private float fireCountdown = 0f;
        [SerializeField] private Enemy currentTarget;

        private IAttackStrategy attackStrategy;
        private readonly Collider[] hitBuffer = new Collider[20];

        // Optimization: Update target only periodically, not every frame.
        private const float TARGET_UPDATE_INTERVAL = 0.2f;
        private float targetUpdateTimer = 0f;

        /// <summary>
        /// Initializes the tower with a specific configuration and strategy.
        /// </summary>
        /// <param name="config">The tower configuration.</param>
        /// <param name="strategy">The specific attack strategy implementation.</param>
        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
        }

        private void Start()
        {
            if (firePoint == null)
            {
                firePoint = transform;
            }

            // Fallback strategy initialization if not injected via Initialize
            if (config != null && attackStrategy == null)
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
                        Debug.LogWarning($"Strategy {config.strategyType} not handled. Defaulting to Laser.");
                        attackStrategy = new LaserAttackStrategy();
                        break;
                }
            }
        }

        private void Update()
        {
            if (config == null) return;

            // Optimize: Update target periodically
            targetUpdateTimer -= Time.deltaTime;
            if (targetUpdateTimer <= 0f)
            {
                UpdateTarget();
                targetUpdateTimer = TARGET_UPDATE_INTERVAL;
            }

            if (currentTarget != null)
            {
                // Check if target is dead or out of range (fast check)
                if (!IsTargetValid())
                {
                    currentTarget = null;
                }
                else
                {
                    if (fireCountdown <= 0f)
                    {
                        attackStrategy?.Attack(currentTarget, firePoint, config);
                        fireCountdown = 1f / config.fireRate;
                    }
                }
            }

            fireCountdown -= Time.deltaTime;
        }

        private bool IsTargetValid()
        {
            if (currentTarget == null) return false;
            if (!currentTarget.gameObject.activeInHierarchy) return false; // Handled by pool disable

            float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            return distance <= config.range + 0.5f; // Small buffer to avoid flickering at edge
        }

        private void UpdateTarget()
        {
            // Use NonAlloc to avoid GC allocation every check
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                if (hitBuffer[i].TryGetComponent<Enemy>(out var enemy))
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }

            if (nearestEnemy != null && shortestDistance <= config.range)
            {
                currentTarget = nearestEnemy;
            }
            else
            {
                currentTarget = null;
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
