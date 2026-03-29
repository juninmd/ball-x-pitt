# 🤖 NeonDefense (Unity Edition)

[![Unity Status](https://img.shields.io/badge/Unity-6000.x-blue.svg?logo=unity)]()
[![Status: Active](https://img.shields.io/badge/Status-Active-brightgreen.svg)]()
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> A Cyberpunk-themed Tower Defense game built with Unity. Defend the central server (Core) from waves of "virus" enemies using high-tech security towers.

## ✨ Features

- **Cyberpunk Aesthetic**: Neon, dark, and high-tech visual style.
- **Strategic Gameplay**: Place towers strategically on the grid to defend against predefined enemy paths.
- **Dynamic Towers**: Configure range, fire rate, and strategies via ScriptableObjects.
- **Cross-Platform**: Optimized for PC (Windows 64-bit) and WebGL deployment.
- **Antigravity Verification**: Built-in scripts for scene and asset integrity validation.

## 🛠️ Tech Stack

- **Game Engine**: Unity 6000+
- **Scripting**: C# (Strictly typed, SOLID, Data/View separation)
- **Architecture**:
  - **Managers**: GameManager, WaveManager, EconomyManager.
  - **Event Driven**: Uses C# Actions for decoupled communication.
  - **Design Patterns**: Object Pooling (Zero GC), Factory Method, Strategy Pattern (Laser, Missile, Slow).
  - **ScriptableObjects**: Data-driven setup for enemies, towers, and waves.

## 🚀 Getting Started

```bash
# Clone the repository
git clone --depth 1 git@github.com:juninmd/NeonDefense.git

# Open the project in Unity Hub
# Import required Packages (Settings > Package Manager)
```

## 🛡️ Antigravity Protocol (Unity)

This project adheres to the **Antigravity** engineering standards:
- **Modular C# Scripts**: Separation of concerns between Logic, View, and Data.
- **Zero Allocations**: Essential Object Pooling for Enemies and Projectiles to prevent GC spikes during gameplay.
- **Scriptable Objects**: Data-driven architecture for enemies, towers and wave management.

---

*"The best offense is a good neon defense."*
