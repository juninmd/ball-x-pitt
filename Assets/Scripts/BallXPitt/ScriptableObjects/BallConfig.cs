using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics")]
        public float mass = 1f;
        public float bounciness = 0.8f;
        public PhysicsMaterial2D physicsMaterial;

        [Header("Visuals")]
        public GameObject prefab;
        public ParticleSystem collisionVFXPrefab;

        [Header("Gameplay")]
        public int baseScore = 10;
    }
}
