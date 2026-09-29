// Generates Object Pooling for Ball-X-Pitt
using System.Collections.Generic;
using UnityEngine;
using BallXPitt.ScriptableObjects;

namespace BallXPitt.Core
{
    public class BallPool : MonoBehaviour
    {
        public static BallPool Instance { get; private set; }

        private Dictionary<int, Queue<Ball>> _ballPool = new Dictionary<int, Queue<Ball>>();
        private Dictionary<int, Queue<ParticleSystem>> _vfxPool = new Dictionary<int, Queue<ParticleSystem>>();

        // Mapeamentos para reciclagem de VFX no Update sem Garbage Collection
        private List<ParticleSystem> _activeVFX = new List<ParticleSystem>();
        private Dictionary<ParticleSystem, int> _activeVFXToKey = new Dictionary<ParticleSystem, int>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void PreAllocate(BallConfig config, int count)
        {
            int key = config.GetInstanceID();
            if (!_ballPool.ContainsKey(key))
                _ballPool[key] = new Queue<Ball>();

            for (int i = 0; i < count; i++)
            {
                Ball ball = Instantiate(config.prefab, transform);
                ball.gameObject.SetActive(false);
                _ballPool[key].Enqueue(ball);
            }
        }

        public Ball GetBall(BallConfig config, Vector3 position)
        {
            int key = config.GetInstanceID();
            if (!_ballPool.ContainsKey(key) || _ballPool[key].Count == 0)
                PreAllocate(config, 1);

            Ball ball = _ballPool[key].Dequeue();
            ball.transform.position = position;
            ball.gameObject.SetActive(true);
            return ball;
        }

        public void ReturnToPool(Ball ball, BallConfig config)
        {
            ball.gameObject.SetActive(false);
            _ballPool[config.GetInstanceID()].Enqueue(ball);
        }

        public void PlayVFX(ParticleSystem prefab, Vector2 position)
        {
            if (prefab == null) return;
            int key = prefab.GetInstanceID();
            if (!_vfxPool.ContainsKey(key))
                _vfxPool[key] = new Queue<ParticleSystem>();

            ParticleSystem vfx = _vfxPool[key].Count > 0 ? _vfxPool[key].Dequeue() : Instantiate(prefab, transform);
            vfx.transform.position = position;
            vfx.gameObject.SetActive(true);
            vfx.Play();

            _activeVFX.Add(vfx);
            _activeVFXToKey[vfx] = key;
        }

        private void Update()
        {
            for (int i = _activeVFX.Count - 1; i >= 0; i--)
            {
                var vfx = _activeVFX[i];
                if (!vfx.IsAlive(true))
                {
                    vfx.gameObject.SetActive(false);
                    _vfxPool[_activeVFXToKey[vfx]].Enqueue(vfx);
                    _activeVFXToKey.Remove(vfx);
                    _activeVFX.RemoveAt(i);
                }
            }
        }
    }
}
