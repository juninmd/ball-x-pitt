using UnityEngine;
using System.Collections.Generic;

namespace NeonDefense.BallXPitt.Core
{
    [DisallowMultipleComponent]
    public class BallPool : MonoBehaviour
    {
        private Dictionary<int, Queue<Ball>> _poolDictionary = new Dictionary<int, Queue<Ball>>();

        public void PreAllocate(Ball prefab, int count)
        {
            int key = prefab.GetInstanceID();
            if (!_poolDictionary.ContainsKey(key))
            {
                _poolDictionary[key] = new Queue<Ball>();
            }

            for (int i = 0; i < count; i++)
            {
                Ball newObj = Instantiate(prefab, transform);
                newObj.gameObject.SetActive(false);
                _poolDictionary[key].Enqueue(newObj);
            }
        }

        public Ball Get(Ball prefab)
        {
            int key = prefab.GetInstanceID();
            if (_poolDictionary.ContainsKey(key) && _poolDictionary[key].Count > 0)
            {
                Ball obj = _poolDictionary[key].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            // Fallback if not pre-allocated enough
            Ball fallbackObj = Instantiate(prefab, transform);
            fallbackObj.gameObject.SetActive(true);
            return fallbackObj;
        }

        public void ReturnToPool(Ball obj, Ball prefab)
        {
            obj.gameObject.SetActive(false);
            int key = prefab.GetInstanceID();
            if (!_poolDictionary.ContainsKey(key))
            {
                _poolDictionary[key] = new Queue<Ball>();
            }
            _poolDictionary[key].Enqueue(obj);
        }
        public static BallPool Instance { get; private set; }

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

        private void OnEnable()
        {
            Ball.OnBallDestroyed += HandleBallDestroyed;
        }

        private void OnDisable()
        {
            Ball.OnBallDestroyed -= HandleBallDestroyed;
        }

        private void HandleBallDestroyed(Ball ball)
        {
            if (ball.Config != null && ball.Config.prefab != null)
            {
                ReturnToPool(ball, ball.Config.prefab);
            }
            else
            {
                // Fallback in case config is missing, though unlikely in a properly set up project
                ball.gameObject.SetActive(false);
            }
        }
    }
}
