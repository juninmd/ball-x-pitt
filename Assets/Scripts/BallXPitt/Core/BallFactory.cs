using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public static class BallFactory
    {
        public static Ball CreateBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config == null || BallPool.Instance == null)
            {
                Debug.LogWarning("BallFactory: Missing BallConfig or BallPool instance.");
                return null;
            }

            Ball newBall = BallPool.Instance.Get(config, position, rotation);
            if (newBall != null)
            {
                newBall.Initialize(config);
            }

            return newBall;
        }
    }
}
