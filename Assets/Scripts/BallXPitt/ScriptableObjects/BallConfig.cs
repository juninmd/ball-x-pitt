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

        [Header("Visuals & Spawning")]
        public Ball prefab;

        [Header("Gameplay Stats")]
        public int baseScore = 10;
    }
}
