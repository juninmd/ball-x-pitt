using UnityEngine;

public class MissileAttackStrategy : IAttackStrategy
{
    public void Attack(Transform target, Transform origin)
    {
        if (ProjectilePool.Instance == null)
        {
            Debug.LogError("ProjectilePool not found!");
            return;
        }

        Projectile projectile = ProjectilePool.Instance.Get();
        projectile.transform.position = origin.position;
        projectile.Initialize(target);
    }
}
