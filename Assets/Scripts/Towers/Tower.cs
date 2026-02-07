// NeonDefense Core System
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting and delegates attacks to an IAttackStrategy.
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

        // NonAlloc buffer for OverlapSphere
        private readonly Collider[] hitBuffer = new Collider[20];

        /// <summary>
        /// Initializes the tower with configuration and a specific strategy.
        /// </summary>
        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
        }

        private void Start()
        {
            // Fallback initialization if set via Inspector
            if (config != null && attackStrategy == null)
            {
                InitializeStrategyInternal();
            }
        }

        private void InitializeStrategyInternal()
        {
             // Simple factory logic for standalone usage
            switch (config.strategyType)
            {
                case AttackStrategyType.Laser:
                    attackStrategy = new LaserAttackStrategy();
                    break;
                case AttackStrategyType.Missile:
                    attackStrategy = new MissileAttackStrategy();
                    break;
                default:
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
            // Using OverlapSphereNonAlloc is more efficient for GC
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                Collider hit = hitBuffer[i];
                if (hit == null) continue;

                Enemy enemyComponent = hit.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemyComponent;
                    }
                }

                // Clear reference immediately to avoid holding onto destroyed objects
                hitBuffer[i] = null;
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
