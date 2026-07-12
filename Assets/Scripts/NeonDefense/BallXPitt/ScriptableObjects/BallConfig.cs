using UnityEngine;

namespace NeonDefense.BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "NeonDefense/BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics")]
        [Range(0.1f, 10f)]
        public float mass = 1f;

        [Range(0f, 1f)]
        public float bounciness = 0.8f;

        [Header("Gameplay")]
        public int baseValue = 100;

        [Header("View/Visuals")]
        public Core.Ball prefab;
    }
}
