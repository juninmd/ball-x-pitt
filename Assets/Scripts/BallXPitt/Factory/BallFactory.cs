using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;
using BallXPitt.Pools;

namespace BallXPitt.Factory
{
    public class BallFactory : MonoBehaviour
    {
        public static BallFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Ball CreateBall(BallConfig config, Vector3 position)
        {
            if (BallPool.Instance == null)
            {
                Debug.LogError("BallPool is missing!");
                return null;
            }

            Ball ball = BallPool.Instance.Get(config);
            if (ball != null)
            {
                ball.transform.position = position;
                ball.Initialize(config);
                GameEvents.OnBallSpawned?.Invoke(ball);
            }
            return ball;
        }
    }
}
