using System.Collections.Generic;
using UnityEngine;
using NeonDefense.Core;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Enemies
{
    /// <summary>
    /// Base class for all enemies. Handles movement along waypoints and health management.
    /// Interacts with EnemyPool for recycling.
    /// </summary>
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
                // If path has at least 2 points, target index 1. Else stay at 0 (or target 0 if only 1).
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
                // Snap to waypoint
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
            // Notify system to award bits
            GameEvents.OnEnemyKilled?.Invoke(this, bitDrop);
            ReturnToPool();
        }

        private void ReachGoal()
        {
            isDead = true;
            // Notify system to deduct player health
            GameEvents.OnEnemyReachedGoal?.Invoke(this, damageToPlayer);
            ReturnToPool();
        }

        private void ReturnToPool()
        {
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
