# NeonDefense

A Cyberpunk Tower Defense game developed in Unity.

## Overview
NeonDefense is a strategy game where players defend a core against waves of enemies using various towers. The game features a modular architecture with ScriptableObjects for configuration and Event-Driven communication.

## Getting Started

### Prerequisites
- Unity 2022.3 LTS or later.

### Setup
1. Clone the repository.
2. Open the project in Unity.
3. See [SETUP.md](SETUP.md) for detailed configuration instructions on creating Enemies, Towers, and Waves.

## Project Structure
- `Assets/Scripts/Core`: Core systems (Events, Pools).
- `Assets/Scripts/Managers`: Game Logic Managers (Wave, Economy, Health).
- `Assets/Scripts/Enemies`: Enemy logic.
- `Assets/Scripts/Towers`: Tower and Projectile logic.
- `Assets/Scripts/Strategies`: Attack strategies (Laser, Missile).
- `Assets/Scripts/ScriptableObjects`: Configuration data definitions.

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

## License
MIT
