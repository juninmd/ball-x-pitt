using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New LevelConfig", menuName = "BallXPitt/Level Config", order = 2)]
    public class LevelConfig : ScriptableObject
    {
        [Header("Level Constraints")]
        [Tooltip("Maximum number of balls available for this level.")]
        public int maxBalls = 10;

        [Tooltip("Target score required to complete the level.")]
        public int targetScore = 5000;

        [Header("Level Layout")]
        [Tooltip("Prefab representing the physical layout of the level (pit, obstacles, etc.).")]
        public GameObject levelLayoutPrefab;
    }
}
