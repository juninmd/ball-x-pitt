using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Base class for all towers. Handles targeting logic using Physics.OverlapSphereNonAlloc
    /// and delegates attack behavior to an injected IAttackStrategy.
    /// </summary>
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerConfig config;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask enemyLayer;

        private float fireCountdown = 0f;
        private Enemy currentTarget;
        private IAttackStrategy attackStrategy;
        private readonly Collider[] hitBuffer = new Collider[20];

        private const float TARGET_UPDATE_INTERVAL = 0.2f;
        private float targetUpdateTimer = 0f;

        public void Initialize(TowerConfig config, IAttackStrategy strategy)
        {
            this.config = config;
            this.attackStrategy = strategy;
        }

        private void Start()
        {
            if (firePoint == null) firePoint = transform;

            // Instancia a estratégia (Poderia vir de uma TowerFactory)
            if (config != null && attackStrategy == null)
            {
                switch (config.strategyType)
                {
                    case AttackStrategyType.Laser: attackStrategy = new LaserAttackStrategy(); break;
                    case AttackStrategyType.Missile: attackStrategy = new MissileAttackStrategy(); break;
                    default: attackStrategy = new LaserAttackStrategy(); break;
                }
            }
        }

        private void Update()
        {
            if (config == null) return;

            targetUpdateTimer -= Time.deltaTime;
            if (targetUpdateTimer <= 0f)
            {
                UpdateTarget();
                targetUpdateTimer = TARGET_UPDATE_INTERVAL;
            }

            if (currentTarget != null)
            {
                if (!IsTargetValid())
                {
                    currentTarget = null;
                }
                else if (fireCountdown <= 0f)
                {
                    attackStrategy?.Attack(currentTarget, firePoint, config);
                    fireCountdown = 1f / config.fireRate;
                }
            }

            fireCountdown -= Time.deltaTime;
        }

        private bool IsTargetValid()
        {
            if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy) return false;
            return Vector3.Distance(transform.position, currentTarget.transform.position) <= config.range + 0.5f;
        }

        private void UpdateTarget()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, config.range, hitBuffer, enemyLayer);
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < count; i++)
            {
                if (hitBuffer[i].TryGetComponent<Enemy>(out var enemy) && enemy.gameObject.activeInHierarchy)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }
            currentTarget = (nearestEnemy != null && shortestDistance <= config.range) ? nearestEnemy : null;
        }
    }
}
