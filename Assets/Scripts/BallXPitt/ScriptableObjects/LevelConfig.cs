using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Level Settings")]
        public int levelId = 1;
        public int maxBalls = 10;
        public int scoreToWin = 5000;

        [Header("Spawn Settings")]
        public float spawnHeight = 10f;
        public float minX = -5f;
        public float maxX = 5f;
    }
}
