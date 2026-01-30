using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T prefab;
    [SerializeField] protected int initialSize = 20;

    protected Queue<T> pool = new Queue<T>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewInstance();
        }
    }

    private T CreateNewInstance()
    {
        if (prefab == null)
        {
            Debug.LogError($"Pool {name} is missing a prefab!");
            return null;
        }
        T instance = Instantiate(prefab, transform);
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
        return instance;
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            CreateNewInstance();
        }

        T instance = pool.Dequeue();
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);
        pool.Enqueue(instance);
    }
}
