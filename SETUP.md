# NeonDefense Setup Guide

## 1. Configuring ScriptableObjects

This project uses ScriptableObjects for data-driven configuration. Follow these steps to set up your first wave.

### Step 1: Create Enemy Config
1. Right-click in the Project window (e.g., inside an `Assets/Data/Enemies` folder).
2. Select `Create -> NeonDefense -> EnemyConfig`.
3. Name it (e.g., `BasicVirus`).
4. Set the values:
   - **Enemy Name**: "Virus Alpha"
   - **Prefab**: Assign your Enemy prefab (must have `Enemy` script).
   - **Health**: 100
   - **Speed**: 5
   - **Bit Drop**: 10
   - **Damage To Player**: 1

### Step 2: Create Tower Config
1. Right-click in Project window.
2. Select `Create -> NeonDefense -> TowerConfig`.
3. Name it (e.g., `LaserTurret`).
4. Set the values:
   - **Tower Name**: "Laser Mk1"
   - **Cost**: 100
   - **Strategy Type**: `Laser` or `Missile`
   - **Range**: 10
   - **Fire Rate**: 2
   - **Prefab**: Assign your Tower prefab.
   - **Projectile Prefab**: (Required if Strategy is Missile).

### Step 3: Create Wave Config
1. Right-click in Project window.
2. Select `Create -> NeonDefense -> WaveConfig`.
3. Name it `Wave01`.
4. In the `Enemy Groups` list, click `+` to add a group:
   - **Enemy Config**: Drag your `BasicVirus` config here.
   - **Count**: 10 (number of enemies).
   - **Spawn Rate**: 0.5 (seconds between each enemy spawn).
   - **Time Between Groups**: 2 (seconds before next group/wave logic).

### Step 4: Scene Setup
1. Create a `GameManager` GameObject.
2. Attach the `WaveManager` script.
   - **Waves**: Add your `WaveConfig` assets.
   - **Waypoints**: Assign Transforms representing the path.
3. Ensure `EnemyPool` and `ProjectilePool` scripts are present in the scene (e.g., on a `Pools` GameObject or the Manager).

---

## 2. GitHub Actions Secrets

To enable the automated build pipeline (Windows & WebGL), add the following Secrets to your GitHub repository:

Go to **Settings -> Secrets and variables -> Actions -> New repository secret**.

| Secret Name | Value |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). You can obtain this by activating a license locally or via `game-ci` CLI. |
| `UNITY_EMAIL` | The email address for your Unity account. |
| `UNITY_PASSWORD` | The password for your Unity account. |

**Trigger:** The build runs automatically when you push a tag starting with `v` (e.g., `git tag v1.0`, `git push origin v1.0`).
