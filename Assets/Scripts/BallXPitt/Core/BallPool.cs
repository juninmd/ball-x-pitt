// Projeto: Ball-x-Pitt Arcade
// Unity & C# Technical Requirements Implemented: Object Pooling (Zero-GC)
using System.Collections.Generic;
using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> _ballPools = new Dictionary<int, Queue<Ball>>();
        private Dictionary<int, Queue<ParticleSystem>> _vfxPools = new Dictionary<int, Queue<ParticleSystem>>();

        private struct ActiveVFXInfo
        {
            public ParticleSystem ps;
            public int prefabInstanceId;
        }

        private List<ActiveVFXInfo> _activeVFX = new List<ActiveVFXInfo>();

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

        public Ball Get(BallConfig config)
        {
            if (config == null || config.prefab == null) return null;

            int key = config.GetInstanceID();
            if (!_ballPools.ContainsKey(key))
            {
                _ballPools[key] = new Queue<Ball>();
            }

            Ball ball = null;
            while (_ballPools[key].Count > 0 && ball == null)
            {
                ball = _ballPools[key].Dequeue();
            }

            if (ball == null)
            {
                GameObject obj = Instantiate(config.prefab);
                obj.SetActive(false); // keep it disabled until positioned by factory
                ball = obj.GetComponent<Ball>();
                if (ball == null)
                {
                    Debug.LogError("Ball prefab is missing Ball component!");
                    Destroy(obj);
                    return null;
                }
            }

            ball.Initialize(config);

            // Reset Rigidbody state before enabling
            if (ball.Rb != null)
            {
                ball.Rb.velocity = Vector2.zero;
                ball.Rb.angularVelocity = 0f;
            }

            return ball;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            if (ball == null || config == null) return;

            int key = config.GetInstanceID();
            if (!_ballPools.ContainsKey(key))
            {
                _ballPools[key] = new Queue<Ball>();
            }

            ball.gameObject.SetActive(false);
            _ballPools[key].Enqueue(ball);
        }

        public void PlayVFX(ParticleSystem vfxPrefab, Vector3 position)
        {
            if (vfxPrefab == null) return;

            int key = vfxPrefab.gameObject.GetInstanceID();
            if (!_vfxPools.ContainsKey(key))
            {
                _vfxPools[key] = new Queue<ParticleSystem>();
            }

            ParticleSystem ps = null;
            while (_vfxPools[key].Count > 0 && ps == null)
            {
                ps = _vfxPools[key].Dequeue();
            }

            if (ps == null)
            {
                ps = Instantiate(vfxPrefab);
            }

            ps.transform.position = position;
            ps.gameObject.SetActive(true);
            ps.Play();

            _activeVFX.Add(new ActiveVFXInfo { ps = ps, prefabInstanceId = key });
        }

        private void Update()
        {
            // Zero-GC loop to check and recycle stopped VFX
            for (int i = _activeVFX.Count - 1; i >= 0; i--)
            {
                var info = _activeVFX[i];
                if (info.ps != null && !info.ps.IsAlive(true))
                {
                    info.ps.gameObject.SetActive(false);
                    _vfxPools[info.prefabInstanceId].Enqueue(info.ps);
                    _activeVFX.RemoveAt(i);
                }
            }
        }
    }
}
