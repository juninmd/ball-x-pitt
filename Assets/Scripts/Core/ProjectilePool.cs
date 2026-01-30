using UnityEngine;

public class ProjectilePool : ObjectPool<Projectile>
{
    public static ProjectilePool Instance { get; private set; }

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            base.Awake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
