using System;

public static class GameEvents
{
    // Event triggered when an enemy is killed.
    // Param 1: The Enemy script (for object pooling return logic if needed elsewhere)
    // Param 2: The bit drop amount
    public static Action<Enemy, int> OnEnemyKilled;

    // Event triggered when an enemy reaches the goal.
    // Param 1: The Enemy script
    // Param 2: The damage amount
    public static Action<Enemy, int> OnEnemyReachedGoal;

    public static Action OnGameOver;

    // Wave events
    public static Action OnWaveStart;
    public static Action OnWaveEnd;
}
