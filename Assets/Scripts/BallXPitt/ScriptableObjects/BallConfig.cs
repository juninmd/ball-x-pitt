using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Física e Status")]
        public float mass = 1f;

        [Header("Prefabs")]
        public GameObject prefab;
        public ParticleSystem collisionVFXPrefab;

        [Header("Gameplay")]
        public int baseScore = 100;
    }
}
