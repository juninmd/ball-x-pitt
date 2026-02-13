# NeonDefense Setup Instructions

## 1. Configuring Game Data (ScriptableObjects)

### creating Enemy Configurations
1. In the Project window, right-click and select `Create -> NeonDefense -> EnemyConfig`.
2. Name the file (e.g., `BasicEnemy`).
3. In the Inspector:
   - Assign the **Prefab** (must have `Enemy` script).
   - Set **Health** (e.g., 100).
   - Set **Speed** (e.g., 5).
   - Set **Bit Drop** (e.g., 10).

### Creating Wave Configurations
1. Right-click and select `Create -> NeonDefense -> WaveConfig`.
2. Name the file (e.g., `Wave1`).
3. In the Inspector:
   - Expand **Enemy Groups**.
   - Add a new Element.
   - Assign an `EnemyConfig`.
   - Set **Count** (e.g., 10).
   - Set **Spawn Rate** (e.g., 0.5 for half a second between spawns).
   - Adjust **Time Between Groups** if needed.

### Creating Tower Configurations
1. Right-click and select `Create -> NeonDefense -> TowerConfig`.
2. Name the file (e.g., `LaserTower`).
3. In the Inspector:
   - Assign the **Prefab** (must have `Tower` script).
   - Set **Strategy Type** (Laser or Missile).
   - Set **Range**, **Fire Rate**, and **Cost**.

## 2. Scene Setup

1. **Managers:**
   - Create an empty GameObject named `Managers`.
   - Add `GameManager`, `WaveManager`, `EconomyManager`, `GridManager`, and `TowerPlacementManager` scripts.
2. **Pooling:**
   - Create an empty GameObject named `Pooling`.
   - Add `ProjectilePool` and `EnemyPool` scripts.
   - Assign the default Prefabs to the pools in the Inspector.
3. **WaveManager Setup:**
   - In the `WaveManager` component, assign your `WaveConfig` assets to the **Waves** list.
   - Assign the Waypoint Transforms (path for enemies) to the **Waypoints** list.
4. **TowerPlacementManager Setup:**
   - Assign the **Default Tower Config** (e.g., your `LaserTower` config).
   - Ensure `Placement Layer` is set to the layer used for the ground/grid.

## 3. GitHub Secrets for CI/CD

To enable the automated build and release pipeline, go to your GitHub Repository -> **Settings** -> **Secrets and variables** -> **Actions** and add the following repository secrets:

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). You can get this by activating a license manually or using game-ci docs. |
| `UNITY_EMAIL` | The email address associated with your Unity ID. |
| `UNITY_PASSWORD` | The password for your Unity ID. |

**Note:** The build workflow (`deploy.yml`) will trigger automatically when you push a tag starting with `v` (e.g., `git tag v1.0 && git push origin v1.0`).
