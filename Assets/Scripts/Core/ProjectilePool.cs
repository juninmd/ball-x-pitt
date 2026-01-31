using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile>
{
    public static ProjectilePool Instance { get; private set; }

    protected override void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Initialize pool from base class
        base.Awake();
    }
}
