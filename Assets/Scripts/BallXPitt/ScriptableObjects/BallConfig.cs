using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Config/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics")]
        public float mass = 1.0f;
        public float bounciness = 0.8f;

        [Header("Visual")]
        public GameObject prefab;

        [Header("Gameplay")]
        public int baseScore = 100;
    }
}