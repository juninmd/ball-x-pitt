using UnityEngine;

public interface IAttackStrategy
{
    void ExecuteAttack(Transform origin, Transform target, TowerConfig config);
}
