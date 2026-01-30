using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private float health;
    private float speed;
    private int bitDrop;

    private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    public void Initialize(EnemyConfig config, List<Transform> path)
    {
        health = config.health;
        speed = config.speed;
        bitDrop = config.bitDrop;
        waypoints = path;
        currentWaypointIndex = 0;

        // Reset state
        gameObject.SetActive(true);

        if (waypoints != null && waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Count) return;

        Transform targetWP = waypoints[currentWaypointIndex];
        Vector3 dir = (targetWP.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWP.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                ReachGoal();
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Notify system. Manager will handle returning to pool.
        GameEvents.OnEnemyKilled?.Invoke(this, bitDrop);
        gameObject.SetActive(false);
    }

    private void ReachGoal()
    {
        // Logic for reaching the core (damage player)
        // For now, just recycle
        Die();
    }
}
