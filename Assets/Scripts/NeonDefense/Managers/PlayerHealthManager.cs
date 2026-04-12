using UnityEngine;
using System;
using NeonDefense.Core;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public class PlayerHealthManager : MonoBehaviour
    {
        public static PlayerHealthManager Instance { get; private set; }

        [SerializeField] private int startingHealth = 20;

        public int CurrentHealth { get; private set; }

        public event Action<int> OnHealthChanged;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }


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

        private void HandleEnemyReachedGoal(Enemy enemy, int damage)
        {
            TakeDamage(damage);
        }

        public void TakeDamage(int amount)
        {
            if (CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            if (CurrentHealth < 0) CurrentHealth = 0;

            OnHealthChanged?.Invoke(CurrentHealth);
            Debug.Log($"Player took {amount} damage. Current Health: {CurrentHealth}");

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player Health Depleted! Game Over.");
            GameEvents.OnGameOver?.Invoke();
        }
    }
}
