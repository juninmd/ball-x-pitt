# NeonDefense Setup Instructions

## 1. Configuring ScriptableObjects for the First Wave

Follow these steps to set up your first wave of enemies in the Unity Editor:

### Step A: Create Enemy Config
1. In the Project window, navigate to `Assets/Scripts/ScriptableObjects/` (or your preferred data folder).
2. Right-click and select **Create -> NeonDefense -> EnemyConfig**.
3. Name the file `BasicEnemy`.
4. In the Inspector:
   - **Enemy Name**: "Virus V1"
   - **Prefab**: Drag your Enemy prefab (must have `Enemy` script attached).
   - **Health**: 10
   - **Speed**: 3
   - **Bit Drop**: 5
   - **Damage To Player**: 1

### Step B: Create Wave Config
1. Right-click and select **Create -> NeonDefense -> WaveConfig**.
2. Name the file `Wave_01`.
3. In the Inspector:
   - **Time Between Groups**: 2 (seconds).
   - **Enemy Groups**: Click "+" to add a group.
     - **Enemy Config**: Drag `BasicEnemy` here.
     - **Count**: 5 (number of enemies).
     - **Spawn Rate**: 1 (seconds between each enemy spawn).

### Step C: Configure WaveManager
1. Select the `WaveManager` GameObject in your scene.
2. Locate the `WaveManager` script component.
3. **Waves**: Set size to 1 and drag `Wave_01` into the element slot.
4. **Waypoints**: Drag your scene waypoints (Transforms) into this list in order (Start -> End).
5. **Auto Start**: Check this if you want the wave to start immediately on Play.

---

## 2. GitHub Secrets for DevOps

To enable the automated build and release pipeline, add the following Secrets to your GitHub repository settings (`Settings -> Secrets and variables -> Actions`):

| Secret Name | Description |
| :--- | :--- |
| `UNITY_LICENSE` | **Recommended.** The content of your `Unity_v20xx.x.ulf` license file. <br>To get this: Run `ulf-extract` or activate manually locally and copy the content of `C:\ProgramData\Unity\Unity_vX.X.ulf` (Windows) or `/Library/Application Support/Unity/Unity_vX.X.ulf` (Mac). |
| `UNITY_EMAIL` | The email address associated with your Unity ID. |
| `UNITY_PASSWORD` | The password for your Unity ID. |

> **Note:** Ideally, use `UNITY_LICENSE` for stability. If you provide `UNITY_LICENSE`, the builder will use it. If not, it will try to activate using Email/Password (which may require 2FA handling or fail more often).
