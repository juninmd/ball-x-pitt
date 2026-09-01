using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public LevelConfig initialLevel;

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