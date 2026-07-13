using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/Ball Config")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics Settings")]
        [Range(0.1f, 10f)]
        public float mass = 1f;

        [Range(0f, 1f)]
        public float bounciness = 0.8f;

        [Header("Visual Settings")]
        public GameObject prefab;

        [Header("Gameplay Settings")]
        public int baseScore = 100;
    }
}
