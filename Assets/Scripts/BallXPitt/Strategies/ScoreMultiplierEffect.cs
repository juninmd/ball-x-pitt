using UnityEngine;
using BallXPitt.Core;
using BallXPitt.Managers;

namespace BallXPitt.Strategies
{
    public class ScoreMultiplierEffect : MonoBehaviour, IEffectStrategy
    {
        [SerializeField] private float multiplier = 1.5f;

        public void ApplyEffect(Ball ball, Collision2D collision)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.ApplyMultiplier(multiplier);
                Debug.Log($"ScoreMultiplierEffect applied! Multiplier: {multiplier}");
            }
        }
    }
}
// Update for Ball-x-Pitt PR
