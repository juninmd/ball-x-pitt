using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public class DestroyBallEffect : MonoBehaviour, IEffectStrategy
    {
        public void ApplyEffect(Ball ball, Collision2D collision)
        {
            ball.Despawn();
            Debug.Log("DestroyBallEffect applied!");
        }
    }
}
