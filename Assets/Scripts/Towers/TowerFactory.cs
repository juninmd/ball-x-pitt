using UnityEngine;
using NeonDefense.Strategies;
using NeonDefense.ScriptableObjects;

namespace NeonDefense.Towers
{
    /// <summary>
    /// Factory responsible for creating Towers and injecting their strategies.
    /// </summary>
    public class TowerFactory : MonoBehaviour
    {
        /// <summary>
        /// Instantiates a tower based on the provided configuration.
        /// </summary>
        /// <param name="config">The configuration data for the tower.</param>
        /// <param name="position">The world position to place the tower.</param>
        /// <returns>The created Tower instance, or null if failed.</returns>
        public Tower CreateTower(TowerConfig config, Vector3 position)
        {
            if (config == null || config.prefab == null)
            {
                 Debug.LogWarning("Invalid TowerConfig!");
                 return null;
            }

            GameObject instance = Instantiate(config.prefab, position, Quaternion.identity);
            Tower tower = instance.GetComponent<Tower>();

            if (tower != null)
            {
                IAttackStrategy strategy = CreateStrategy(config.strategyType);
                tower.Initialize(config, strategy);
            }
            else
            {
                Debug.LogError($"Prefab {config.prefab.name} does not have a Tower component!");
            }

            return tower;
        }

        private IAttackStrategy CreateStrategy(AttackStrategyType type)
        {
            switch (type)
            {
                case AttackStrategyType.Laser: return new LaserAttackStrategy();
                case AttackStrategyType.Missile: return new MissileAttackStrategy();
                default: return new LaserAttackStrategy();
            }
        }
    }
}
