using System.Collections;
using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        private TowerConfig config;
        private IAttackStrategy attackStrategy;

        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private Enemy currentTarget;
        private float fireCooldown;
        private Collider[] hitColliders = new Collider[20];

        public void Initialize(TowerConfig towerConfig, IAttackStrategy strategy)
        {
            this.attackStrategy = strategy;
            this.config = towerConfig;
            StartCoroutine(UpdateTarget());
        }

        private void Start()
        {
            // Optional fallback if not initialized by a factory
            if (attackStrategy == null && config != null)
            {
                switch (config.attackStrategyType)
                {
                    case AttackStrategyType.Laser:
                        attackStrategy = new LaserAttackStrategy();
                        break;
                    case AttackStrategyType.Missile:
                        attackStrategy = new MissileAttackStrategy();
                        break;
                }
                StartCoroutine(UpdateTarget());
            }
        }

        private void Update()
        {
            if (fireCooldown > 0f)
            {
                fireCooldown -= Time.deltaTime;
            }

            if (currentTarget != null && fireCooldown <= 0f && attackStrategy != null)
            {
                attackStrategy.Attack(currentTarget, firePoint, config);
                fireCooldown = 1f / config.fireRate;
            }
        }

        private IEnumerator UpdateTarget()
        {
            while (true)
            {
                FindTarget();
                yield return new WaitForSeconds(0.2f); // Optimized target acquisition
            }
        }

        private void FindTarget()
        {
            if (currentTarget != null && currentTarget.gameObject.activeInHierarchy && Vector3.Distance(transform.position, currentTarget.transform.position) <= config.range)
            {
                return; // Current target is still valid
            }

            currentTarget = null;

            // Zero GC allocation approach
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitColliders, enemyLayer);

            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < numColliders; i++)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    if (hitColliders[i].TryGetComponent<Enemy>(out var enemy) && enemy.gameObject.activeInHierarchy)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }

            currentTarget = nearestEnemy;
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