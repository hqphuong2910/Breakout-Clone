using System;
using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Events
{
    public static class GameEvents
    {
        public static Action<GameState> OnGameStateChanged;
        public static Action OnInitializing;
        public static Action OnMainMenu;
        public static Action OnGameStarted;
        public static Action OnLevelCompleted;
        public static Action OnGameOver;

        public static Action<Transform> OnPaddleReady;

        public static Action OnBallDropped;

        public static Action<int> OnBrickDestroyed;

        public static Action<int> OnLivesChanged;
        public static Action<int> OnScoreChanged;
    }
}