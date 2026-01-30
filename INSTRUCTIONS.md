# NeonDefense Project Instructions

## 1. Unity Project Setup (ScriptableObjects)

To configure the game data, you need to create ScriptableObject assets in the Project window.

### Creating Enemy Configs
1. Right-click in the Project window: `Create -> NeonDefense -> EnemyConfig`.
2. Name the file (e.g., `BasicVirus`).
3. Set the attributes:
   - **Name**: "Virus Alpha"
   - **Health**: 100
   - **Speed**: 5
   - **Bit Drop**: 10
   - **Prefab**: Assign your Enemy prefab (must have `Enemy` script attached).

### Creating Tower Configs
1. Right-click: `Create -> NeonDefense -> TowerConfig`.
2. Name the file (e.g., `LaserTurret`).
3. Set attributes:
   - **Cost**: 100
   - **Range**: 10
   - **Fire Rate**: 2 (shots per second)
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

### Scene Setup
1. Create an empty GameObject named `WaveManager`.
   - Attach `WaveManager` script.
   - Assign your `WaveConfig` assets to the **Waves** list.
   - Assign Waypoints (Transformers representing the path).
   - Create a child object with `EnemyPool` script (or attach `EnemyPool` to the manager) and assign it to the `Enemy Pool` field.
2. Create an empty GameObject named `EconomyManager`.
   - Attach `EconomyManager` script.
   - Set starting bits.

## 2. DevOps & GitHub Actions

To enable automated builds (Windows & WebGL) and Releases:

### Required GitHub Secrets
Go to your repository **Settings -> Secrets and variables -> Actions** and add:

1. `UNITY_LICENSE`
   - The content of your Unity License file (`.ulf`).
   - If using a Personal license, you may need to acquire this via the `game-ci` activation steps locally or use their documentation to export it.
2. `UNITY_EMAIL`
   - Your Unity account email.
3. `UNITY_PASSWORD`
   - Your Unity account password.

### Triggering a Build
The pipeline runs **only** when you push a tag starting with `v`.

```bash
git add .
git commit -m "feat: Ready for release"
git tag v1.0
git push origin v1.0
```

Check the **Actions** tab in GitHub to see the build progress.
