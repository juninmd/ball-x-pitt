using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;
// using NeonDefense.Managers; // If we needed pools here directly

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerConfig config;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private IAttackStrategy attackStrategy;
        private float fireCountdown = 0f;
        private Enemy currentTarget;

        private void Start()
        {
            if (config == null)
            {
                Debug.LogError("TowerConfig not assigned!");
                return;
            }

            InitializeStrategy();
        }

        private void InitializeStrategy()
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

        // Helper to inject strategy if needed (Factory pattern)
        public void SetStrategy(IAttackStrategy strategy)
        {
            this.attackStrategy = strategy;
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
            // Simple closest target logic
            Collider[] hits = Physics.OverlapSphere(transform.position, config.range, enemyLayer);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var hit in hits)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = hit.gameObject;
                }
            }

            if (nearestEnemy != null && shortestDistance <= config.range)
            {
                currentTarget = nearestEnemy.GetComponent<Enemy>();
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
