using System.Collections;
using UnityEngine;
using NeonDefense.Enemies;
using NeonDefense.ScriptableObjects;
using NeonDefense.Strategies;

namespace NeonDefense.Towers
{
    public class Tower : MonoBehaviour
    {
        private TowerConfig _config;
        private IAttackStrategy _attackStrategy;

        [SerializeField] private Transform _firePoint;

        private Enemy _currentTarget;
        private float _fireTimer = 0f;

        private Collider[] _targetBuffer = new Collider[20];

        public void Initialize(IAttackStrategy attackStrategy, TowerConfig config)
        {
            _attackStrategy = attackStrategy;
            _config = config;

            StartCoroutine(UpdateTarget());
        }

        private void Update()
        {
            if (_currentTarget == null || _attackStrategy == null || _config == null)
            {
                return;
            }

            // Look at target
            Vector3 direction = _currentTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

            _fireTimer += Time.deltaTime;
            if (_fireTimer >= 1f / _config.fireRate)
            {
                _fireTimer = 0f;
                _attackStrategy.Attack(_currentTarget, _firePoint, _config);
            }
        }

        private IEnumerator UpdateTarget()
        {
            while (true)
            {
                FindNearestTarget();
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void FindNearestTarget()
        {
            if (_config == null) return;

            int hits = Physics.OverlapSphereNonAlloc(transform.position, _config.range, _targetBuffer);
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;

            for (int i = 0; i < hits; i++)
            {
                Enemy enemy = _targetBuffer[i].GetComponent<Enemy>();
                if (enemy != null)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }

            _currentTarget = nearestEnemy;
        }

        private void OnDrawGizmosSelected()
        {
            if (_config != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, _config.range);
            }
        }
    }
}
