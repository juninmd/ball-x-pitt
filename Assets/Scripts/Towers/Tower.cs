using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private TowerConfig config;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private IAttackStrategy attackStrategy;
        private float fireCountdown = 0f;
        private Enemy currentTarget;

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
            Collider[] hits = Physics.OverlapSphere(transform.position, config.range, enemyLayer);
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            foreach (var hit in hits)
            {
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
            if (attackStrategy != null)
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
