using UnityEngine;

namespace NeonDefense.Core
{
    [DisallowMultipleComponent]
    public class ProjectilePool : ObjectPool<Projectile>
    {
        private static ProjectilePool _instance;

        public static ProjectilePool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ProjectilePool>();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("ProjectilePool");
                        _instance = obj.AddComponent<ProjectilePool>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
