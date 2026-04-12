using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    /// <summary>
    /// Manages the player's currency (Bits).
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private int startingBits = 100;

        private int currentBits;

        /// <summary>
        /// Gets the current amount of bits.
        /// </summary>
        public int CurrentBits => currentBits;

        /// <summary>
        /// Event triggered when the bit count changes.
        /// </summary>
        public System.Action<int> OnBitsChanged;

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
        }

        private void Start()
        {
            currentBits = startingBits;
            OnBitsChanged?.Invoke(currentBits);
        }

        private void OnEnable()
        {
            GameEvents.OnEnemyKilled += HandleEnemyKilled;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        }

        private void HandleEnemyKilled(Enemy enemy, int amount)
        {
            AddBits(amount);
        }

        private void AddBits(int amount)
        {
            currentBits += amount;
            OnBitsChanged?.Invoke(currentBits);
        }

        /// <summary>
        /// Attempts to spend the specified amount of bits.
        /// </summary>
        /// <param name="amount">Amount to spend.</param>
        /// <returns>True if purchase was successful, false otherwise.</returns>
        public bool SpendBits(int amount)
        {
            if (currentBits >= amount)
            {
                currentBits -= amount;
                OnBitsChanged?.Invoke(currentBits);
                return true;
            }
            return false;
        }
    }
}
