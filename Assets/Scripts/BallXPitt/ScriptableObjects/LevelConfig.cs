using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "BallXPitt/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public int maxBalls = 10;
        public int scoreToWin = 1000;

        [Header("Spawn Boundaries")]
        public float minX = -4f;
        public float maxX = 4f;
        public float spawnHeight = 10f;

        public GameObject layoutPrefab;
    }
}
// Update for Ball-x-Pitt PR
