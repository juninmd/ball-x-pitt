using System.Collections.Generic;
using UnityEngine;
using BallXPitt.Core;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Pools
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> poolDictionary = new Dictionary<int, Queue<Ball>>();

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

        public void PreAllocate(BallConfig config, int count)
        {
            int id = config.GetInstanceID();
            if (!poolDictionary.ContainsKey(id))
            {
                poolDictionary[id] = new Queue<Ball>();
            }

            for (int i = 0; i < count; i++)
            {
                Ball ball = CreateNewBall(config);
                ball.gameObject.SetActive(false);
                poolDictionary[id].Enqueue(ball);
            }
        }

        private Ball CreateNewBall(BallConfig config)
        {
            GameObject obj = Instantiate(config.prefab);
            obj.transform.SetParent(transform);
            Ball ball = obj.GetComponent<Ball>();
            if (ball == null)
            {
                ball = obj.AddComponent<Ball>();
            }
            return ball;
        }

        public Ball Get(BallConfig config)
        {
            int id = config.GetInstanceID();
            if (poolDictionary.ContainsKey(id) && poolDictionary[id].Count > 0)
            {
                Ball ball = poolDictionary[id].Dequeue();
                ball.gameObject.SetActive(true);
                return ball;
            }

            Ball newBall = CreateNewBall(config);
            newBall.gameObject.SetActive(true);
            return newBall;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            ball.gameObject.SetActive(false);
            int id = config.GetInstanceID();
            if (!poolDictionary.ContainsKey(id))
            {
                poolDictionary[id] = new Queue<Ball>();
            }
            poolDictionary[id].Enqueue(ball);
        }

        private struct ActiveVFXInfo
        {
            public ParticleSystem particleSystem;
            public int prefabId;
        }

        private Dictionary<int, Queue<ParticleSystem>> vfxPoolDictionary = new Dictionary<int, Queue<ParticleSystem>>();
        private List<ActiveVFXInfo> activeVFXList = new List<ActiveVFXInfo>();

        public void PlayVFX(ParticleSystem vfxPrefab, Vector2 position)
        {
            if (vfxPrefab == null) return;

            int id = vfxPrefab.GetInstanceID();
            if (!vfxPoolDictionary.ContainsKey(id))
            {
                vfxPoolDictionary[id] = new Queue<ParticleSystem>();
            }

            ParticleSystem ps = null;
            if (vfxPoolDictionary[id].Count > 0)
            {
                ps = vfxPoolDictionary[id].Dequeue();
                ps.transform.position = position;
                ps.gameObject.SetActive(true);
            }
            else
            {
                ps = Instantiate(vfxPrefab, position, Quaternion.identity);
                ps.transform.SetParent(transform);
            }

            ps.Play();
            activeVFXList.Add(new ActiveVFXInfo { particleSystem = ps, prefabId = id });
        }

        private void Update()
        {
            for (int i = activeVFXList.Count - 1; i >= 0; i--)
            {
                ActiveVFXInfo info = activeVFXList[i];

                if (info.particleSystem != null && !info.particleSystem.IsAlive(true))
                {
                    info.particleSystem.gameObject.SetActive(false);
                    vfxPoolDictionary[info.prefabId].Enqueue(info.particleSystem);
                    activeVFXList.RemoveAt(i);
                }
            }
        }
    }
}
