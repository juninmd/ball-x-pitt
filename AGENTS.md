# AGENTS.md

## Context
NeonDefense is a Unity Tower Defense game. The codebase uses `NeonDefense.*` namespaces.
Key patterns: Object Pooling, Strategy Pattern, ScriptableObjects, Event-Driven Architecture.

## Guidelines
- **Namespaces:** Always use `NeonDefense.Core`, `NeonDefense.Managers`, `NeonDefense.Enemies`, etc.
- **Pooling:** Use `ObjectPool<T>` for spawned entities.
- **Config:** Use ScriptableObjects for data.
- **Events:** Use `GameEvents` in `Core` for cross-system communication.

## Future Roadmap
1. **Implement Damage Types & Armor:** Add a system where specific towers deal bonus damage to specific enemy armor types (e.g., Laser vs Shield, Missile vs Hull).
2. **UI Manager:** Create a dedicated `UIManager` to decouple UI updates from `EconomyManager` and `PlayerHealthManager`. Currently, they handle logic but rely on external listeners for UI.
3. **Endless Mode:** specific WaveConfig generation logic to support infinite scaling waves.
