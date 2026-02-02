# NeonDefense Setup Guide

## 1. Configuring ScriptableObjects

This project uses ScriptableObjects for data-driven configuration. Follow these steps to set up your first wave.

### Step 1: Create Enemy Config
1. In the Project window, navigate to `Assets/Data/Enemies` (create folders if needed).
2. Right-click and select `Create -> NeonDefense -> EnemyConfig`.
3. Name it (e.g., `BasicVirus`).
4. Set the values:
   - **Name**: "Virus Alpha"
   - **Health**: 100
   - **Speed**: 5
   - **Bit Drop**: 10
   - **Prefab**: Assign your Enemy prefab.

### Step 2: Create Tower Config
1. Navigate to `Assets/Data/Towers`.
2. Right-click and select `Create -> NeonDefense -> TowerConfig`.
3. Name it (e.g., `LaserTurret`).
4. Set the values:
   - **Cost**: 100
   - **Range**: 10
   - **Fire Rate**: 2
   - **Strategy Type**: Laser
   - **Prefab**: Assign your Tower prefab.
   - **Projectile Prefab**: Assign if using Missile strategy.

### Step 3: Create Wave Config
1. Navigate to `Assets/Data/Waves`.
2. Right-click and select `Create -> NeonDefense -> WaveConfig`.
3. Name it `Wave01`.
4. In the `Enemy Groups` list, add a new element:
   - **Enemy Config**: Drag `BasicVirus` here.
   - **Count**: 10
   - **Spawn Rate**: 1 (spawn every 1 second).
   - **Time Between Groups**: 0 (if only one group).

### Step 4: Scene Setup (Managers)
1. Create an empty GameObject named `GameManager` (or similar).
2. Attach `WaveManager` and `EconomyManager` scripts.
3. In `WaveManager`:
   - Populate `Waves` list with your `WaveConfig` assets.
   - Populate `Waypoints` list with Transform objects representing the path.

### Step 5: Scene Setup (Pools)
1. Create an empty GameObject named `Pools`.
2. Attach `EnemyPool` script.
   - Assign the **Enemy Prefab** to the pool.
   - Set `Initial Pool Size`.
3. Attach `ProjectilePool` script.
   - Assign the **Projectile Prefab** to the pool.
   - Set `Initial Pool Size`.

---

## 2. GitHub Actions Secrets

To enable the automated build pipeline, you must add the following Secrets to your GitHub repository:

Go to **Settings -> Secrets and variables -> Actions -> New repository secret**.

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). You can obtain this by activating a license locally or via `game-ci` CLI. |
| `UNITY_EMAIL` | The email address associated with your Unity ID. |
| `UNITY_PASSWORD` | The password for your Unity ID. |

**Note:** The pipeline triggers automatically when you push a tag starting with `v` (e.g., `git tag v1.0 && git push origin v1.0`).
