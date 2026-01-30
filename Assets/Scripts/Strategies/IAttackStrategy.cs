using UnityEngine;

public interface IAttackStrategy
{
    void Attack(Transform target, Transform origin);
}
