# NeonDefense Setup Guide

This guide details how to configure the project, set up assets, and deploy using GitHub Actions.

## 1. Project Configuration (Unity)

### Creating ScriptableObjects
The game data is driven by ScriptableObjects. Here's how to create them:

#### Enemy Configuration
1. In the Project window, right-click in a folder (e.g., `Assets/Data/Enemies`).
2. Select **Create -> NeonDefense -> EnemyConfig**.
3. Name the file (e.g., `BasicEnemy`).
4. In the Inspector, configure:
   - **Enemy Name**: "Virus Basic"
   - **Prefab**: Assign your enemy prefab (must have `Enemy` script attached).
   - **Health**: 10
   - **Speed**: 3
   - **Bit Drop**: 5
   - **Damage To Player**: 1

#### Wave Configuration
1. In the Project window, right-click in a folder (e.g., `Assets/Data/Waves`).
2. Select **Create -> NeonDefense -> WaveConfig**.
3. Name the file (e.g., `Wave01`).
4. In the Inspector:
   - Expand **Enemy Groups**.
   - Set Size to 1 (or more).
   - For Element 0:
     - **Enemy Config**: Drag your `BasicEnemy` asset here.
     - **Count**: 5 (number of enemies).
     - **Spawn Rate**: 1.5 (seconds between spawns).
   - **Time Between Groups**: Delay before next group/wave logic.

### Scene Setup
1. **Managers**: Create an empty GameObject named `GameManagers`.
   - Add `WaveManager` script.
   - Assign your `WaveConfig` assets to the **Waves** list.
   - Create child objects for Waypoints and assign them to the **Waypoints** list in order.
   - Add `EconomyManager` and `GameManager` scripts (if available).
2. **Pools**: Create an empty GameObject named `Pools`.
   - Add `ProjectilePool` script. Assign a projectile prefab.
   - Add `EnemyPool` script (if separate) or ensure `WaveManager` references a pool.
3. **Towers**:
   - Create Tower Prefabs with the `Tower` script attached.
   - Create `TowerConfig` assets (**Create -> NeonDefense -> TowerConfig**) and assign them to the Tower scripts.

## 2. GitHub Secrets (CI/CD)

To enable automated builds and releases, you must configure the following Secrets in your GitHub repository settings (**Settings -> Secrets and variables -> Actions**):

| Secret Name | Description |
|---|---|
| `UNITY_LICENSE` | The content of your Unity License File (`.ulf`). |
| `UNITY_EMAIL` | The email address associated with your Unity ID. |
| `UNITY_PASSWORD` | The password for your Unity ID. |

**How to get UNITY_LICENSE:**
1. Allows Unity to activate in "Offline Mode" which is more reliable for CI.
2. You can extract this from a local machine activation or use the `game-ci` documentation to generate one.

## 3. Triggering a Release

The deployment workflow runs automatically when you push a tag starting with `v`.

```bash
git tag v1.0.0
git push origin v1.0.0
```

This will:
1. Build for Windows 64-bit and WebGL.
2. Create a GitHub Release.
3. Upload `NeonDefense_Windows.zip` and `NeonDefense_WebGL.zip` as assets.
