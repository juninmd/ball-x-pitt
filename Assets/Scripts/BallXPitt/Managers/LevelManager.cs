// Projeto: Ball-x-Pitt Arcade
// Unity & C# Technical Requirements Implemented: Managers, Event Driven, SOLID
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;
using BallXPitt.Factories;

namespace BallXPitt.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [SerializeField] private LevelConfig currentLevelConfig;
        [SerializeField] private BallConfig defaultBallConfig;

        private int _ballsRemaining;
        private int _activeBalls;
        private bool _levelActive;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void OnEnable()
        {
            GameEvents.OnBallSpawned += HandleBallSpawned;
            GameEvents.OnBallDestroyed += HandleBallDestroyed;
        }

        private void OnDisable()
        {
            GameEvents.OnBallSpawned -= HandleBallSpawned;
            GameEvents.OnBallDestroyed -= HandleBallDestroyed;
        }

        public void StartLevel(LevelConfig config)
        {
            if (config != null)
            {
                currentLevelConfig = config;
            }

            if (currentLevelConfig == null)
            {
                Debug.LogError("No LevelConfig assigned!");
                return;
            }

            _ballsRemaining = currentLevelConfig.maxBalls;
            _activeBalls = 0;
            _levelActive = true;

            // Optionally instantiate layout prefab here if not already in scene
            if (currentLevelConfig.layoutPrefab != null)
            {
                // Simple instantiation for demo; in full game, consider destroying previous layout
                Instantiate(currentLevelConfig.layoutPrefab, Vector3.zero, Quaternion.identity);
            }

            GameEvents.OnLevelStarted?.Invoke(1); // Passing dummy level index 1
        }

        private void Update()
        {
            if (!_levelActive || currentLevelConfig == null) return;

            // Simple gameplay input: left click to drop a ball
            if (Input.GetMouseButtonDown(0) && _ballsRemaining > 0)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float clampedX = Mathf.Clamp(mousePos.x, currentLevelConfig.minX, currentLevelConfig.maxX);
                Vector3 spawnPos = new Vector3(clampedX, currentLevelConfig.spawnHeight, 0f);

                BallFactory.CreateBall(defaultBallConfig, spawnPos);
                _ballsRemaining--;
            }
        }

        private void HandleBallSpawned(Ball ball)
        {
            _activeBalls++;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            _activeBalls--;
            CheckLevelCompletion();
        }

        private void CheckLevelCompletion()
        {
            if (!_levelActive) return;

            if (_activeBalls <= 0 && _ballsRemaining <= 0)
            {
                _levelActive = false;

                // Example win condition: check score via ScoreManager
                if (ScoreManager.Instance != null && ScoreManager.Instance.TotalScore >= currentLevelConfig.scoreToWin)
                {
                    Debug.Log("Level Won!");
                    GameEvents.OnLevelCompleted?.Invoke();
                }
                else
                {
                    Debug.Log("Game Over!");
                    GameEvents.OnGameOver?.Invoke();
                }
            }
        }
    }
}
