using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;
using NeonDefense.Managers; // For EnemyPool access if needed

namespace NeonDefense.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private float health;
        private float speed;
        private int bitDrop;
        private int damageToPlayer;

        private List<Transform> waypoints;
        private int waypointIndex = 0;

        private bool isDead = false;

        public void Initialize(EnemyConfig config, List<Transform> path)
        {
            this.health = config.health;
            this.speed = config.speed;
            this.bitDrop = config.bitDrop;
            this.damageToPlayer = config.damageToPlayer;
            this.waypoints = path;
            this.isDead = false;

            // Reset position to first waypoint and target the next one
            if (waypoints != null && waypoints.Count > 0)
            {
                transform.position = waypoints[0].position;
                this.waypointIndex = (waypoints.Count > 1) ? 1 : 0;
            }
            else
            {
                this.waypointIndex = 0;
            }
        }

        private void Update()
        {
            if (isDead) return;

            Move();
        }

        private void Move()
        {
            if (waypoints == null || waypointIndex >= waypoints.Count) return;

            Transform targetWaypoint = waypoints[waypointIndex];
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            float distance = speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetWaypoint.position) <= distance)
            {
                transform.position = targetWaypoint.position;
                waypointIndex++;

                if (waypointIndex >= waypoints.Count)
                {
                    ReachGoal();
                }
            }
            else
            {
                transform.Translate(direction * distance, Space.World);
            }
        }

        public void TakeDamage(float amount)
        {
            if (isDead) return;

            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            GameEvents.OnEnemyKilled?.Invoke(this, bitDrop);

            // Return to pool
            if (EnemyPool.Instance != null)
            {
                EnemyPool.Instance.ReturnToPool(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ReachGoal()
        {
            isDead = true; // Technically not dead, but done interacting
            GameEvents.OnEnemyReachedGoal?.Invoke(this, damageToPlayer);

            if (EnemyPool.Instance != null)
            {
                EnemyPool.Instance.ReturnToPool(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
