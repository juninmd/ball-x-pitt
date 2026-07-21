using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New BallConfig", menuName = "BallXPitt/Ball Config", order = 1)]
    public class BallConfig : ScriptableObject
    {
        [Header("Physics Settings")]
        public float mass = 1f;
        [Range(0f, 1f)]
        public float bounciness = 0.8f;

        [Header("Visuals")]
        public GameObject prefab;

        [Header("Scoring")]
        public int baseScore = 100;
    }
}
