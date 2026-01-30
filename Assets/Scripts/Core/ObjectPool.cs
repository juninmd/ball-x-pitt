using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T prefab;
    [SerializeField] protected int initialSize = 10;

    protected Queue<T> pool = new Queue<T>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            T instance = CreateNewInstance();
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    protected T CreateNewInstance()
    {
        T instance = Instantiate(prefab, transform);
        return instance;
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            return CreateNewInstance();
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
