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

        public void PreAllocate(BallConfig config, int amount)
        {
            if (config == null || config.prefab == null) return;

            int key = config.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            for (int i = 0; i < amount; i++)
            {
                Ball newBall = Instantiate(config.prefab, transform);
                newBall.gameObject.SetActive(false);
                pools[key].Enqueue(newBall);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config == null) return null;

            int key = config.GetInstanceID();

            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            Ball ball;
            if (pools[key].Count > 0)
            {
                ball = pools[key].Dequeue();
                ball.transform.position = position;
                ball.transform.rotation = rotation;
            }
            else
            {
                ball = Instantiate(config.prefab, position, rotation, transform);
            }

            ball.gameObject.SetActive(true);
            return ball;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            ball.gameObject.SetActive(false);

            int key = config.GetInstanceID();
            if (!pools.ContainsKey(key))
            {
                pools[key] = new Queue<Ball>();
            }

            pools[key].Enqueue(ball);
        }
    }
}
