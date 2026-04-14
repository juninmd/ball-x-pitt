using UnityEngine;

namespace NeonDefense.Core
{
    public class Projectile : MonoBehaviour
    {
        private Projectile prefabSource;
        private Enemies.Enemy target;
        private int damage;
        private float speed = 20f;

        private float lifeTimer;
        private float maxLifetime = 5f;

        public void Initialize(Enemies.Enemy target, int damage, Projectile sourcePrefab)
        {
            this.target = target;
            this.damage = damage;
            this.prefabSource = sourcePrefab;
            this.lifeTimer = 0f;
        }

        private void Update()
        {
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                ReturnToPool();
                return;
            }

            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                HitTarget();
            }

            lifeTimer += Time.deltaTime;
            if (lifeTimer >= maxLifetime)
            {
                ReturnToPool();
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
            if (ProjectilePool.Instance != null && prefabSource != null)
            {
                ProjectilePool.Instance.ReturnToPool(this, prefabSource);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
