using UnityEngine;
using System;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private int startingBits = 100;

    public int CurrentBits { get; private set; }

    public event Action<int> OnBitsChanged;

    private void Awake()
    {
        CurrentBits = startingBits;
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled(Enemy enemy, int bits)
    {
        AddBits(bits);
    }

    public void AddBits(int amount)
    {
        CurrentBits += amount;
        OnBitsChanged?.Invoke(CurrentBits);
    }

    public bool TrySpendBits(int amount)
    {
        if (CurrentBits >= amount)
        {
            CurrentBits -= amount;
            OnBitsChanged?.Invoke(CurrentBits);
            return true;
        }
        return false;
    }
}
