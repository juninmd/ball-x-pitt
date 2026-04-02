using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;
using System.Collections;

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayerMask;

        private TowerConfig config;
        private IAttackStrategy attackStrategy;
        private Enemy currentTarget;
        private float fireCountdown = 0f;

        private Collider[] targetBuffer = new Collider[20];

        public void Initialize(TowerConfig towerConfig, IAttackStrategy strategy)
        {
            this.config = towerConfig;
            this.attackStrategy = strategy;

            StartCoroutine(UpdateTarget());
        }

        private IEnumerator UpdateTarget()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                FindTarget();
                yield return wait;
            }
        }

        private void FindTarget()
        {
            // Reset current target if it's dead, inactive or out of range
            if (currentTarget != null)
            {
                if (!currentTarget.gameObject.activeInHierarchy ||
                    Vector3.Distance(transform.position, currentTarget.transform.position) > config.range)
                {
                    currentTarget = null;
                }
                else
                {
                    // Target is still valid
                    return;
                }
            }

            // Object Pooling / Zero GC specific optimization: Use OverlapSphereNonAlloc
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, config.range, targetBuffer, enemyLayerMask);

            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < numColliders; i++)
            {
                Enemy enemy = targetBuffer[i].GetComponent<Enemy>();
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }

            currentTarget = nearestEnemy;
        }

        private void Update()
        {
            if (currentTarget == null || config == null || attackStrategy == null) return;

            fireCountdown -= Time.deltaTime;

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / config.fireRate;
            }
        }

        private void Shoot()
        {
            attackStrategy.Attack(currentTarget, firePoint, config);
        }

        private void OnDrawGizmosSelected()
        {
            if (config != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, config.range);
            }
        }
    }
}
