using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Rules")]
        public int maxBalls = 10;
        public int scoreToWin = 1000;

        [Header("Level Data")]
        public GameObject layoutPrefab;

        [Header("Spawn Bounds")]
        public float minX = -5f;
        public float maxX = 5f;
        public float spawnHeight = 10f;
    }
}
// Update for Ball-x-Pitt PR
