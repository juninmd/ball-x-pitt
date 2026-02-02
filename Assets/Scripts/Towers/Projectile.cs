using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.Managers; // For ProjectilePool if needed to return itself, though usually caller handles it.
// Actually simpler: Projectile handles its own lifetime or collision.

namespace NeonDefense.Towers
{
    public class Projectile : MonoBehaviour
    {
        private float speed = 10f;
        private float damage;
        private Enemy target;

        // Callback to return to pool
        private System.Action<Projectile> returnToPoolAction;

        public void Initialize(Enemy target, float damage, System.Action<Projectile> returnToPool)
        {
            this.target = target;
            this.damage = damage;
            this.returnToPoolAction = returnToPool;

            // Safety destroy/return if target is null (shouldn't happen on init)
             if (target == null)
            {
                ReturnToPool();
            }
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
            returnToPoolAction?.Invoke(this);
        }
    }
}
