using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics")]
        public float mass = 1f;
        [Range(0f, 1f)]
        public float bounciness = 0.8f;

        [Header("Visuals")]
        public Ball prefab;

        [Header("Gameplay")]
        public int baseScore = 10;
    }
}
