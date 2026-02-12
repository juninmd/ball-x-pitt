# NeonDefense

A Cyberpunk Tower Defense game developed in Unity, focusing on clean architecture, design patterns, and robust DevOps practices.

## 🏗️ Architecture & Clean Code

This project adheres to **SOLID principles** and isolates Data logic from View logic.

### Core Systems
- **Managers:** Centralized control via `GameManager` (State), `WaveManager` (Spawning), and `EconomyManager` (Currency).
- **Event-Driven:** Uses a lightweight Event Bus (`GameEvents.cs`) to decouple systems. For example, `OnEnemyKilled` triggers economy updates without direct dependencies.

### Design Patterns
1.  **Object Pooling:** Essential for performance. `ProjectilePool` and `EnemyPool` manage reusable instances to minimize Garbage Collection.
2.  **Factory Method:** `TowerFactory` handles the instantiation of towers and injection of strategies.
3.  **Strategy Pattern:** `IAttackStrategy` defines attack behaviors (Laser, Missile), allowing towers to switch logic at runtime or configuration.
4.  **ScriptableObjects:** Used extensively for data configuration (`EnemyConfig`, `TowerConfig`, `WaveConfig`).

## 🚀 DevOps (GitHub Actions)

The project includes a production-ready CI/CD pipeline in `.github/workflows/deploy.yml`.

- **Trigger:** Builds are triggered **only** on tags starting with `v*` (e.g., `v1.0`).
- **Targets:** Builds for **Windows 64-bit** and **WebGL**.
- **License Handling:** Automatically activates Unity Pro/Plus license via GitHub Secrets.
- **Automated Release:** Creates a GitHub Release with changelogs and attaches zipped artifacts for both platforms.

## 🛠️ Setup & Configuration

For detailed instructions on how to configure Waves, Enemies, and Towers in the Unity Editor, please refer to [SETUP.md](SETUP.md).

### Quick Start
1.  Open the project in Unity.
2.  Navigate to `Assets/Scripts/ScriptableObjects/` to create new configurations.
3.  Assign configurations to the `WaveManager` in the scene.
4.  Press Play!

## 📂 Project Structure

- `Assets/Scripts/Core`: Fundamental systems (Pooling, Events).
- `Assets/Scripts/Managers`: Game state and logic controllers.
- `Assets/Scripts/ScriptableObjects`: Data definitions.
- `Assets/Scripts/Strategies`: Attack logic implementations.
- `Assets/Scripts/Towers`: Tower components and factory.
- `Assets/Scripts/Enemies`: Enemy AI and movement.
