using System;
using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Events
{
    public static class GameEvents
    {
        public static Action<GameState> OnGameStateChanged;
        public static Action OnLevelCompleted;
        public static Action OnGameOver;
        public static Action<Transform> OnPaddleReady;
        public static Action OnBallDropped;
        public static Action<int> OnBrickDestroyed;
    }
}