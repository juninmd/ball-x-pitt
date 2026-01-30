using UnityEngine;

public class LaserAttackStrategy : IAttackStrategy
{
    public void Attack(Transform target, Transform origin)
    {
        // Logic to draw laser
        // In a real implementation, we would access a LineRenderer component here.
        Debug.Log($"Laser beaming {target.name} from {origin.name}");
    }
}
