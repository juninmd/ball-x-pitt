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
        private float damage;
        private Enemy target;

        /// <summary>
        /// Initializes the projectile.
        /// </summary>
        /// <param name="target">The enemy to chase.</param>
        /// <param name="damage">Damage to deal on hit.</param>
        public void Initialize(Enemy target, float damage)
        {
            this.target = target;
            this.damage = damage;

            // Auto-return to pool after lifetime if no target hit (fail-safe)
            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 5f);
        }

        private void Update()
        {
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
            CancelInvoke(nameof(ReturnToPool));
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
