using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 1f;
    private Transform target;

    public void Initialize(Transform target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            // Simple check to return to pool if target is lost
            // In a real game we might want to continue or destroy after time
             if (ProjectilePool.Instance != null)
                ProjectilePool.Instance.ReturnToPool(this);
            else
                Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            // Hit logic
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

             if (ProjectilePool.Instance != null)
                ProjectilePool.Instance.ReturnToPool(this);
            else
                Destroy(gameObject);
        }
    }
}
