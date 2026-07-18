using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public LevelConfig initialLevel;
        public BallConfig defaultBall;
        public Transform spawnPoint; // Temporary logic for input

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
            if (initialLevel != null)
            {
                // Pre-allocate some balls
                if (defaultBall != null)
                {
                    BallPool.Instance.PreAllocate(defaultBall, initialLevel.maxBalls);
                }

                LevelManager.Instance.StartLevel(initialLevel);
            }
        }
    }
}