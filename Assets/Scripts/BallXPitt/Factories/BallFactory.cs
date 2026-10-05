using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Factories
{
    public static class BallFactory
    {
        public static Ball CreateBall(BallConfig config, Vector3 position)
        {
            if (config == null || BallPool.Instance == null)
            {
                Debug.LogWarning("Cannot create ball: Missing config or BallPool not initialized.");
                return null;
            }

            Ball ball = BallPool.Instance.Get(config);
            if (ball != null)
            {
                ball.transform.position = position;
                ball.gameObject.SetActive(true);
            }
            return ball;
        }
    }
}
