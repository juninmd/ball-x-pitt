using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics Settings")]
        public float mass = 1f;
        public float bounciness = 0.8f;

        [Header("Visual & Prefab Settings")]
        public GameObject prefab;

        [Header("Game Data")]
        public int baseScore = 10;
    }
}
