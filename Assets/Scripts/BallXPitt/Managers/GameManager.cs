using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public BallXPitt.ScriptableObjects.LevelConfig initialLevelConfig;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (initialLevelConfig != null && LevelManager.Instance != null)
            {
                LevelManager.Instance.StartLevel(initialLevelConfig);
            }
            else
            {
                Debug.LogWarning("GameManager: Missing initialLevelConfig or LevelManager instance to start the game.");
            }
        }

        private void OnEnable()
        {
            GameEvents.OnLevelCompleted += HandleLevelCompleted;
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnLevelCompleted -= HandleLevelCompleted;
            GameEvents.OnGameOver -= HandleGameOver;
        }

        private void HandleLevelCompleted()
        {
            Debug.Log("GameManager: Level Completed!");
        }

        private void HandleGameOver()
        {
            Debug.Log("GameManager: Game Over!");
        }
    }
}
