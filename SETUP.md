# NeonDefense Setup Guide

## 1. Unity Editor Setup (ScriptableObjects)

To run the first wave, you need to configure the data assets in Unity.

### Step A: Create Enemy
1. Right-click in the Project Window -> `Create` -> `NeonDefense` -> `EnemyConfig`.
2. Name it `BasicEnemy`.
3. Set properties:
   - **Enemy Name**: Virus
   - **Prefab**: Assign your Enemy prefab.
   - **Health**: 100
   - **Speed**: 5
   - **Bit Drop**: 10
   - **Damage To Player**: 1

### Step B: Create Tower
1. Right-click -> `Create` -> `NeonDefense` -> `TowerConfig`.
2. Name it `LaserTower`.
3. Set properties:
   - **Tower Name**: Laser Tower
   - **Cost**: 100
   - **Strategy Type**: Laser
   - **Range**: 10
   - **Fire Rate**: 1

### Step C: Create Wave
1. Right-click -> `Create` -> `NeonDefense` -> `WaveConfig`.
2. Name it `Wave01`.
3. In **Enemy Groups**, click `+`:
   - **Enemy Config**: Drag `BasicEnemy` here.
   - **Count**: 10
   - **Spawn Rate**: 1.0

### Step D: Scene Configuration
1. Select the `GameManager` (or `WaveManager`) GameObject in your scene.
2. In the `WaveManager` component:
   - **Waves**: Add `Wave01` to the list.
   - **Waypoints**: Drag your waypoint Transforms into this list in order.

---

## 2. GitHub Actions Secrets

To enable the automated build and release pipeline, add the following Secrets to your GitHub Repository:
(Settings -> Secrets and variables -> Actions)

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | The content of your Unity License file (`.ulf`). You can get this by activating a Personal license locally or using game-ci docs. |
| `UNITY_EMAIL` | The email address of your Unity ID. |
| `UNITY_PASSWORD` | The password of your Unity ID. |

## 3. Triggering a Release

The build pipeline only runs when you push a specific tag.

```bash
git tag v1.0.0
git push origin v1.0.0
```
