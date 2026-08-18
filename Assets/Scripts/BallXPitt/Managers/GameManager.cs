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
            if (initialLevel != null && LevelManager.Instance != null)
            {
                LevelManager.Instance.StartLevel(initialLevel);
            }
        }
    }
}