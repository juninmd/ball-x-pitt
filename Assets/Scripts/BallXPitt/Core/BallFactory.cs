using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallFactory : MonoBehaviour
    {
        public static BallFactory Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public Ball CreateBall(BallConfig config, Vector3 position)
        {
            if (BallPool.Instance == null)
            {
                Debug.LogError("BallPool instance not found!");
                return null;
            }

            Ball newBall = BallPool.Instance.Get(config, position, Quaternion.identity);
            if (newBall != null)
            {
                newBall.Initialize(config);
            }
            return newBall;
        }
    }
}
