using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics Settings")]
        public float mass = 1f;
        [Range(0f, 1f)]
        public float bounciness = 0.8f;
        public PhysicsMaterial2D physicsMaterial;

        [Header("Visuals")]
        public Ball prefab;
        public ParticleSystem collisionVFXPrefab;

        [Header("Scoring")]
        public int baseScore = 100;
    }
}
