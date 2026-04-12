// NeonDefense Core System
using UnityEngine;
using NeonDefense.Enemies;

namespace NeonDefense.Core
{
    /// <summary>
    /// Handles projectile movement and collision logic.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 5f;

        private float damage;
        private Enemy target;
        private float lifeTimer;

        /// <summary>
        /// Initializes the projectile.
        /// </summary>
        /// <param name="target">The enemy to chase.</param>
        /// <param name="damage">Damage to deal on hit.</param>
        public void Initialize(Enemy target, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.lifeTimer = lifetime;
        }

        private void Update()
        {
            // Handle lifetime
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                ReturnToPool();
                return;
            }

            if (target == null || !target.gameObject.activeInHierarchy)
            {
                ReturnToPool();
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;
            float distanceThisFrame = speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.transform.position) <= distanceThisFrame)
            {
                HitTarget();
            }
            else
            {
                transform.Translate(direction * distanceThisFrame, Space.World);
            }
        }

        private void HitTarget()
        {
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            ReturnToPool();
        }

        private void ReturnToPool()
        {
            if (ProjectilePool.Instance != null)
            {
                ProjectilePool.Instance.ReturnToPool(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
