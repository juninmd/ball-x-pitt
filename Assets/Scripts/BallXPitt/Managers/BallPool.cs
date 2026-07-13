using System.Collections.Generic;
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Managers
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> pools = new Dictionary<int, Queue<Ball>>();
        private Dictionary<int, BallConfig> poolConfigs = new Dictionary<int, BallConfig>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PreAllocate(BallConfig config, int count)
        {
            if (config == null || config.prefab == null)
            {
                Debug.LogError("BallPool: Invalid config or prefab.");
                return;
            }

            int instanceId = config.GetInstanceID();

            if (!pools.ContainsKey(instanceId))
            {
                pools[instanceId] = new Queue<Ball>();
                poolConfigs[instanceId] = config;
            }

            for (int i = 0; i < count; i++)
            {
                Ball ball = CreateNewBall(config);
                ball.gameObject.SetActive(false);
                pools[instanceId].Enqueue(ball);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config == null) return null;

            int instanceId = config.GetInstanceID();

            if (!pools.ContainsKey(instanceId))
            {
                PreAllocate(config, 1);
            }

            Queue<Ball> pool = pools[instanceId];
            Ball ball = null;

            if (pool.Count > 0)
            {
                ball = pool.Dequeue();
            }
            else
            {
                ball = CreateNewBall(config);
            }

            ball.transform.position = position;
            ball.transform.rotation = rotation;
            ball.gameObject.SetActive(true);

            GameEvents.OnBallSpawned?.Invoke(ball);

            return ball;
        }

        public void ReturnBall(Ball ball)
        {
            if (ball == null) return;

            ball.gameObject.SetActive(false);

            if (ball.Config != null)
            {
                int instanceId = ball.Config.GetInstanceID();
                if (pools.ContainsKey(instanceId))
                {
                    pools[instanceId].Enqueue(ball);
                }
            }

            GameEvents.OnBallDespawned?.Invoke(ball);
        }

        private Ball CreateNewBall(BallConfig config)
        {
            GameObject obj = Instantiate(config.prefab, transform);
            Ball ball = obj.GetComponent<Ball>();
            if (ball == null)
            {
                ball = obj.AddComponent<Ball>();
            }
            ball.Initialize(config);
            return ball;
        }
    }
}
