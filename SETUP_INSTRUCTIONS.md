# NeonDefense Setup Instructions

## 1. Configuring ScriptableObjects

To create the first wave, you need to set up `EnemyConfig` and `WaveConfig` assets.

### Creating an Enemy Configuration
1.  In the Project window, right-click in a folder (e.g., `Assets/ScriptableObjects`).
2.  Select **Create -> NeonDefense -> EnemyConfig**.
3.  Name the file (e.g., `VirusBasic`).
4.  In the Inspector:
    -   **Enemy Name:** "Virus"
    -   **Prefab:** Assign your Enemy Prefab (must have `Enemy` script).
    -   **Health:** 10
    -   **Speed:** 5
    -   **Bit Drop:** 5
    -   **Damage To Player:** 1

### Creating a Wave Configuration
1.  Right-click and select **Create -> NeonDefense -> WaveConfig**.
2.  Name the file (e.g., `Wave01`).
3.  In the Inspector:
    -   **Enemy Groups:** Click "+" to add a group.
        -   **Enemy Config:** Drag and drop your `VirusBasic` config.
        -   **Count:** 10 (number of enemies).
        -   **Spawn Rate:** 0.5 (seconds between spawns).
    -   **Time Between Groups:** 2.0 (seconds before next wave if chained).

### Setting up the WaveManager
1.  Select the `WaveManager` GameObject in your scene.
2.  In the `Waves` list, add your `Wave01` config.
3.  Assign your Waypoints to the `Waypoints` list.

## 2. GitHub Secrets for DevOps

To enable the automated build and release pipeline (`deploy.yml`), you must add the following Secrets to your GitHub repository:

1.  Go to **Settings -> Secrets and variables -> Actions**.
2.  Click **New repository secret**.

### Required Secrets:

*   `UNITY_LICENSE`:
    *   This is the XML content of your Unity License file (`.ulf`).
    *   To get this, you need to activate your license via command line or use the [GameCI documentation](https://game.ci/docs/github/activation) to generate it.
    *   *Note:* Use a Personal License if you don't have Pro.

*   `UNITY_EMAIL`:
    *   The email address associated with your Unity ID.

*   `UNITY_PASSWORD`:
    *   The password for your Unity ID.

### Workflow Trigger
The build pipeline only runs when you push a tag starting with `v`.
Example:
```bash
git tag v1.0
git push origin v1.0
```
