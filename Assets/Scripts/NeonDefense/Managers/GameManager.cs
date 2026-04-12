using UnityEngine;
using NeonDefense.Core;
using UnityEngine.SceneManagement;

namespace NeonDefense.Managers
{
    public enum GameState
    {
        Playing,
        GameOver,
        Paused
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; }

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
            CurrentState = GameState.Playing;
        }

        private void OnEnable()
        {
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnGameOver -= HandleGameOver;
        }

        private void HandleGameOver()
        {
            if (CurrentState == GameState.GameOver) return;

            CurrentState = GameState.GameOver;
            Debug.Log("Game Over! Restarting level in 5 seconds...");
            Invoke(nameof(RestartLevel), 5f);
        }

        private void RestartLevel()
        {
            // Simple restart logic
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // In a real scenario, we might want to reset static events or ensure clean state
            // But Scene reload usually handles most cleanup if Managers are set up right
            // Note: DontDestroyOnLoad might cause duplication if not handled in Awake (which it is)
        }
    }
}
