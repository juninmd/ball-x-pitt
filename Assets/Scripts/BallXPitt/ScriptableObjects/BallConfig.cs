// Configuration SO for Physics Ball
using UnityEngine;

namespace BallXPitt.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewBallConfig", menuName = "BallXPitt/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        [Header("Física e Status")]
        public float mass = 1f;

        [Header("Prefabs")]
        public GameObject prefab;
        public GameObject collisionVFXPrefab;

        [Header("Gameplay")]
        public int baseScore = 100;
    }
}
// Generated for BallXPitt
// Update for Ball-x-Pitt PR
