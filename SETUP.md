# NeonDefense Setup Guide

## 1. ScriptableObject Configuration

To set up the game data, use the Unity Editor Project window context menu.

### Creating Enemy Configs
1. Right-click in the Project window: `Create -> NeonDefense -> EnemyConfig`.
2. Name the file (e.g., `BasicVirus`).
3. Set the attributes in the Inspector:
   - **Enemy Name**: e.g., "Virus Alpha"
   - **Prefab**: Assign your Enemy prefab (must have `Enemy` script).
   - **Health**: e.g., 100
   - **Speed**: e.g., 5
   - **Bit Drop**: e.g., 10 (Currency reward)
   - **Damage To Player**: e.g., 1 (Lives lost if it leaks)

### Creating Tower Configs
1. Right-click: `Create -> NeonDefense -> TowerConfig`.
2. Name the file (e.g., `LaserTurret`).
3. Set attributes:
   - **Tower Name**: e.g., "Laser MK1"
   - **Cost**: e.g., 100
   - **Prefab**: The tower model prefab.
   - **Projectile Prefab**: (Optional) Assign if using Missile strategy.
   - **Range**: e.g., 10
   - **Fire Rate**: e.g., 2 (Shots per second)
   - **Strategy Type**: Select `Laser` or `Missile`.

### Creating Wave Configs
1. Right-click: `Create -> NeonDefense -> WaveConfig`.
2. Name the file (e.g., `Wave01`).
3. In **Enemy Groups**, add a new element:
   - **Enemy Config**: Drag your `BasicVirus` asset here.
   - **Count**: 10 (Number of enemies in this group)
   - **Spawn Rate**: 1 (Seconds delay between spawns).
4. **Time Between Groups**: Delay before next group or wave end.

## 2. GitHub Actions Secrets (DevOps)

To enable the automated CI/CD pipeline, go to your GitHub Repository -> Settings -> Secrets and variables -> Actions, and add the following Repository Secrets:

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). |
| `UNITY_EMAIL` | Your Unity account email address. |
| `UNITY_PASSWORD` | Your Unity account password. |

### Triggering a Release
The pipeline triggers automatically when you push a git tag starting with `v`. This will build for Windows and WebGL, and create a GitHub Release with the artifacts.

```bash
git tag v1.0.0
git push origin v1.0.0
```

## 3. Scene Setup

To run the game, ensure your scene contains the following Managers:

1. **GameManager**: Create an empty GameObject `GameManager`, attach `GameManager.cs`.
2. **WaveManager**: Create an empty GameObject `WaveManager`, attach `WaveManager.cs`.
   - Assign your `WaveConfig` assets to the `Waves` list.
   - Assign Waypoint Transforms to the `Waypoints` list.
3. **ProjectilePool**: Create an empty GameObject `ProjectilePool`, attach `ProjectilePool.cs`.
   - Assign a generic Projectile prefab to the `Prefab` field (required for Missile towers).
4. **EnemyPool**: Create an empty GameObject `EnemyPool`, attach `EnemyPool.cs`.
   - Assign a generic Enemy prefab to the `Prefab` field.
