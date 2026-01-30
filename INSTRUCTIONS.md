# NeonDefense Project Setup

## Unity Configuration

### 1. Creating Enemies
1. In the Project window, right-click and select `Create -> NeonDefense -> EnemyConfig`.
2. Name the file (e.g., `BasicVirus`).
3. Set the attributes:
   - **Name**: Virus Alpha
   - **Health**: 100
   - **Speed**: 5
   - **Bit Drop**: 10
   - **Prefab**: Assign your enemy prefab here.

### 2. Creating Waves
1. Right-click and select `Create -> NeonDefense -> WaveConfig`.
2. In the inspector, expand **Enemy Groups**.
3. Add a new element:
   - **Enemy Config**: Drag your `BasicVirus` config here.
   - **Count**: 10 (number of enemies)
   - **Spawn Rate**: 0.5 (seconds between spawns)
4. Set **Time Between Groups** (e.g., 2 seconds).

### 3. Setting up the Scene
1. Create an empty GameObject named `WaveManager`.
2. Attach the `WaveManager` script.
3. In the `Waves` list, drag your `WaveConfig` assets.
4. Assign a **Spawn Point** transform.

## DevOps Setup (GitHub Actions)

To enable the automated build and release pipeline, add the following Secrets to your GitHub repository (`Settings -> Secrets and variables -> Actions`):

1. **UNITY_LICENSE**
   - The content of your Unity License file (`.ulf`).
   - *Tip: You can extract this from a local machine activation or use the `game-ci` activation method to generate it.*

2. **UNITY_EMAIL**
   - The email address associated with your Unity ID.

3. **UNITY_PASSWORD**
   - The password for your Unity ID.

### How to Trigger a Release
1. Commit your changes.
2. Create a tag starting with `v` (e.g., `v1.0.0`).
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```
3. Go to the "Actions" tab in GitHub to watch the build process.
4. Once complete, a new Release will be created with the Windows and WebGL builds attached.
