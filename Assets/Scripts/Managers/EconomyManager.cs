using UnityEngine;
using NeonDefense.Core;
using NeonDefense.Enemies;

namespace NeonDefense.Managers
{
    public class EconomyManager : MonoBehaviour
    {
        public static EconomyManager Instance { get; private set; }

        [SerializeField] private int startingBits = 100;
        private int currentBits;

        public int CurrentBits => currentBits;

        // Event for UI to listen to
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
            GameEvents.OnEnemyKilled += AddBits;
        }

        private void OnDisable()
        {
            GameEvents.OnEnemyKilled -= AddBits;
        }

        private void AddBits(Enemy enemy, int amount)
        {
            currentBits += amount;
            OnBitsChanged?.Invoke(currentBits);
        }

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
