using System;
using UnityEngine;

public static class GameEvents
{
    public static Action OnWaveStart;
    public static Action OnWaveEnd;
    public static Action<int> OnEnemyKilled; // Int passes bit reward
    public static Action OnGameOver;
}
