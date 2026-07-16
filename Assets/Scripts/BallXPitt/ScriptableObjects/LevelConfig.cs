using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Game Rules")]
        public int maxBalls = 20;
        public int targetScore = 500;

        [Header("Level Layout")]
        public GameObject levelPrefab;
        public float difficultyMultiplier = 1f;
    }
}
