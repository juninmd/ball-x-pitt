using UnityEngine;

public class TowerFactory : MonoBehaviour
{
    public GameObject laserTowerPrefab;
    public GameObject missileTowerPrefab;

    public Tower CreateTower(TowerType type, Vector3 position)
    {
        GameObject prefab = null;
        switch (type)
        {
            case TowerType.Laser:
                prefab = laserTowerPrefab;
                break;
            case TowerType.Missile:
                prefab = missileTowerPrefab;
                break;
        }

        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, position, Quaternion.identity);
            return instance.GetComponent<Tower>();
        }

        Debug.LogWarning($"Prefab for tower type {type} is missing!");
        return null;
    }
}
