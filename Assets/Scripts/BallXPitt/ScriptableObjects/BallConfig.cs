// Configuration SO for Physics Ball
using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Física e Status")]
        public float mass = 1f;
        [Range(0f, 1f)]
        public float bounciness = 0.8f;
        public int baseScore = 10;

        [Header("Referências")]
        public Core.Ball prefab;
        public ParticleSystem collisionVFXPrefab;
    }
}
// Generated for BallXPitt
// Update for Ball-x-Pitt PR
