using System.Collections.Generic;
using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> poolDictionary = new Dictionary<int, Queue<Ball>>();
        private Transform poolParent;

        private Dictionary<int, Queue<ParticleSystem>> vfxPoolDictionary = new Dictionary<int, Queue<ParticleSystem>>();
        private List<ParticleSystem> activeVFXList = new List<ParticleSystem>();
        private Dictionary<ParticleSystem, int> vfxToPrefabKeyMap = new Dictionary<ParticleSystem, int>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            poolParent = new GameObject("BallsPool").transform;
            poolParent.SetParent(transform);
        }

        public void PreAllocate(BallConfig config, int amount)
        {
            if (config == null || config.prefab == null) return;

            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<Ball>();
            }

            for (int i = 0; i < amount; i++)
            {
                GameObject newGo = Instantiate(config.prefab, poolParent);
                Ball newBall = newGo.GetComponent<Ball>();
                newBall.gameObject.SetActive(false);
                poolDictionary[key].Enqueue(newBall);
            }

            if (config.collisionVFXPrefab != null)
            {
                int vfxKey = config.collisionVFXPrefab.GetInstanceID();
                if (!vfxPoolDictionary.ContainsKey(vfxKey))
                {
                    vfxPoolDictionary[vfxKey] = new Queue<ParticleSystem>();
                }

                for (int i = 0; i < amount / 2 + 1; i++)
                {
                    ParticleSystem vfx = Instantiate(config.collisionVFXPrefab, poolParent);
                    vfx.gameObject.SetActive(false);
                    vfxPoolDictionary[vfxKey].Enqueue(vfx);
                    vfxToPrefabKeyMap[vfx] = vfxKey;
                }
            }
        }

        public Ball Get(BallConfig config, Vector3 position, Quaternion rotation)
        {
            if (config == null || config.prefab == null) return null;

            int key = config.GetInstanceID();

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary[key] = new Queue<Ball>();
            }

            Ball ballToSpawn;

            if (poolDictionary[key].Count > 0)
            {
                ballToSpawn = poolDictionary[key].Dequeue();
                ballToSpawn.transform.position = position;
                ballToSpawn.transform.rotation = rotation;
            }
            else
            {
                GameObject newGo = Instantiate(config.prefab, position, rotation, poolParent);
                ballToSpawn = newGo.GetComponent<Ball>();
            }

            ballToSpawn.gameObject.SetActive(true);
            return ballToSpawn;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            ball.gameObject.SetActive(false);

            int key = config.GetInstanceID();
            if (poolDictionary.ContainsKey(key))
            {
                poolDictionary[key].Enqueue(ball);
            }
        }

        public void PlayVFX(ParticleSystem vfxPrefab, Vector3 position)
        {
            if (vfxPrefab == null) return;

            int vfxKey = vfxPrefab.GetInstanceID();

            if (!vfxPoolDictionary.ContainsKey(vfxKey))
            {
                vfxPoolDictionary[vfxKey] = new Queue<ParticleSystem>();
            }

            ParticleSystem vfxToPlay;

            if (vfxPoolDictionary[vfxKey].Count > 0)
            {
                vfxToPlay = vfxPoolDictionary[vfxKey].Dequeue();
                vfxToPlay.transform.position = position;
            }
            else
            {
                vfxToPlay = Instantiate(vfxPrefab, position, Quaternion.identity, poolParent);
                vfxToPrefabKeyMap[vfxToPlay] = vfxKey;
            }

            vfxToPlay.gameObject.SetActive(true);
            vfxToPlay.Play();
            activeVFXList.Add(vfxToPlay);
        }

        private void Update()
        {
            for (int i = activeVFXList.Count - 1; i >= 0; i--)
            {
                ParticleSystem vfx = activeVFXList[i];
                if (vfx != null && !vfx.IsAlive(true))
                {
                    vfx.gameObject.SetActive(false);
                    activeVFXList.RemoveAt(i);

                    if (vfxToPrefabKeyMap.TryGetValue(vfx, out int vfxKey))
                    {
                        if (vfxPoolDictionary.ContainsKey(vfxKey))
                        {
                            vfxPoolDictionary[vfxKey].Enqueue(vfx);
                        }
                    }
                }
            }
        }
    }
}