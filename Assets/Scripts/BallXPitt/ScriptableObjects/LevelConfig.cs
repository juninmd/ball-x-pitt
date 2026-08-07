using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Level Settings")]
        public int maxBalls = 10;
        public int scoreToWin = 1000;

        [Header("Difficulty Settings")]
        [Tooltip("Multiplier for the overall difficulty of the level layout or scoring requirements.")]
        public float difficultyMultiplier = 1.0f;
    }
}
