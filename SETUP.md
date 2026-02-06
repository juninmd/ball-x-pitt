# NeonDefense Setup Guide

## 1. ScriptableObject Configuration

To set up the game data, use the Unity Editor Project window.

### Creating Enemy Configs
1. Right-click in the Project window: `Create -> NeonDefense -> EnemyConfig`.
2. Name the file (e.g., `BasicVirus`).
3. Set the attributes:
   - **Name**: e.g., "Virus Alpha"
   - **Health**: e.g., 100
   - **Speed**: e.g., 5
   - **Bit Drop**: e.g., 10
   - **Prefab**: Assign your Enemy prefab (must have `Enemy` script).

### Creating Tower Configs
1. Right-click: `Create -> NeonDefense -> TowerConfig`.
2. Name the file (e.g., `LaserTurret`).
3. Set attributes:
   - **Cost**: e.g., 100
   - **Range**: e.g., 10
   - **Fire Rate**: e.g., 2
   - **Strategy Type**: Select `Laser` or `Missile`.
   - **Prefab**: The tower model prefab.
   - **Projectile Prefab**: (Optional) Assign if using Missile strategy.

### Creating Wave Configs
1. Right-click: `Create -> NeonDefense -> WaveConfig`.
2. Name the file (e.g., `Wave01`).
3. In **Enemy Groups**, add a new element:
   - **Enemy Config**: Drag your `BasicVirus` asset here.
   - **Count**: 10
   - **Spawn Rate**: 1 (second delay between spawns).

## 2. GitHub Actions Secrets

To enable the CI/CD pipeline, add the following Secrets in your GitHub Repository settings:

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). |
| `UNITY_EMAIL` | Your Unity account email address. |
| `UNITY_PASSWORD` | Your Unity account password. |

### Triggering a Release
The pipeline triggers automatically when you push a tag starting with `v`.

```bash
git tag v1.0.0
git push origin v1.0.0
```

## 3. Scene Setup

To run the game, ensure your scene contains the following Managers:

1. **GameManager**: Create an empty GameObject, name it `GameManager`, and attach `GameManager.cs`.
2. **WaveManager**: Create an empty GameObject, name it `WaveManager`, attach `WaveManager.cs`. Assign your `WaveConfig` assets to the `Waves` list. Assign Waypoints.
3. **EconomyManager**: Create an empty GameObject, name it `EconomyManager`, attach `EconomyManager.cs`.
4. **PlayerHealthManager**: Create an empty GameObject (or attach to Core), attach `PlayerHealthManager.cs`.
5. **Pools**:
   - **ProjectilePool**: Create an empty GameObject, name it `ProjectilePool`, attach `ProjectilePool.cs`. Assign a projectile prefab.
   - **EnemyPool**: Create an empty GameObject, name it `EnemyPool`, attach `EnemyPool.cs` (if available) or ensure logic handles instantiation.
