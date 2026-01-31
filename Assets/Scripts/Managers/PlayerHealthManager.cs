using UnityEngine;
using System;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float startingHealth = 100f;

    public float CurrentHealth { get; private set; }

    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = startingHealth;
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyReachedGoal += HandleEnemyReachedGoal;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyReachedGoal -= HandleEnemyReachedGoal;
    }

    private void HandleEnemyReachedGoal(Enemy enemy, float damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            GameEvents.OnGameOver?.Invoke();
            Debug.Log("Game Over!");
        }
    }
}
