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

        private void Update()
        {
            // Simple Input handling for falling balls (Horizontal move and drop)
            if (Input.GetMouseButtonDown(0) && spawnPoint != null && defaultBall != null)
            {
                // Basic implementation of dropping a ball from a selected position
                Vector3 dropPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                dropPosition.y = spawnPoint.position.y; // Keep it at the top of the pit
                dropPosition.z = 0; // 2D plane logic on a 3D setup

                LevelManager.Instance.SpawnBall(defaultBall, dropPosition);
            }
        }
    }
}