using System;

public static class GameEvents
{
    // Event triggered when an enemy is killed.
    // Param 1: The Enemy script (for object pooling return logic if needed elsewhere)
    // Param 2: The bit drop amount
    public static Action<Enemy, int> OnEnemyKilled;

    // Wave events
    public static Action OnWaveStart;
    public static Action OnWaveEnd;
}
