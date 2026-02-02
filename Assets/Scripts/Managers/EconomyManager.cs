using UnityEngine;
using NeonDefense.Core;

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

        private void AddBits(object enemy, int amount) // Matching Action<Enemy, int> but using object/clean signature
        {
            // Note: GameEvents.OnEnemyKilled is Action<Enemy, int>.
            // In C#, we can subscribe with a method that matches signature.
            // But if I defined it as Action<Enemy, int>, I need to match types.
            // I'll fix the signature below.
            AddBitsInternal(amount);
        }

        // Correct signature for the event
        private void AddBits(NeonDefense.Enemies.Enemy enemy, int amount)
        {
            AddBitsInternal(amount);
        }

        private void AddBitsInternal(int amount)
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
