using UnityEngine;
using BallXPitt.Core;

namespace BallXPitt.Strategies
{
    public interface IEffectStrategy
    {
        void ApplyEffect(Ball ball, Collision collision);
    }
}
