using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/Config/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [Header("Level Rules")]
        public int maxBalls = 10;
        public int scoreToWin = 1000;

        [Header("Level Setup")]
        public string levelName = "Level 1";
    }
}