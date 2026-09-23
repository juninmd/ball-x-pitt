using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics")]
        public float mass = 1f;

        [Header("Prefabs")]
        public GameObject prefab;
        public GameObject collisionVFXPrefab;

        [Header("Gameplay")]
        public int baseScore = 100;
    }
}
