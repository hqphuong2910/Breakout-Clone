using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameConfigSO gameConfig;
        [SerializeField] private int maxLives = 3;
        private int _currentScore;
        private int _remainingLives;
        public GameState CurrentState { get; private set; }

        protected override void Start()
        {
            base.Start();

            ResetGameData();
            ChangeState(gameConfig.bypassMainMenu ? GameState.Playing : GameState.MainMenu, false);
        }

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            GameEvents.OnBrickDestroyed += HandleBrickDestroyed;
            GameEvents.OnBallDropped += HandleBallDropped;
            GameEvents.OnLevelCompleted += HandleLevelCompleted;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            GameEvents.OnBrickDestroyed -= HandleBrickDestroyed;
            GameEvents.OnBallDropped -= HandleBallDropped;
            GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        }

        #endregion

        #region GAMEFLOW_HANDLERS

        private void ChangeState(GameState newState, bool force)
        {
            if (!force && CurrentState == newState) return;

            CurrentState = newState;
            switch (CurrentState)
            {
                case GameState.Initializing:
                case GameState.MainMenu:
                case GameState.Playing:
                case GameState.Paused:
                case GameState.LevelCompleted:
                case GameState.GameOver:
                    AppLogger.Log(name, $"Current game state has been set to: {CurrentState}.");
                    break;
                default:
                    AppLogger.LogError(name, "Invalid game state.");
                    break;
            }

            GameEvents.OnGameStateChanged?.Invoke(CurrentState);
        }

        private void ResetGameData()
        {
            _remainingLives = maxLives;
            AppLogger.Log(name, $"Remaining lives: {_remainingLives}.");

            _currentScore = 0;
            AppLogger.Log(name, $"Current score: {_currentScore}.");

            AppLogger.Log(name, "Reset game data.");
        }

        private void GameOver()
        {
            ChangeState(GameState.GameOver, false);
            GameEvents.OnGameOver?.Invoke();
        }

        public void ReloadGame()
        {
            ResetGameData();
            Time.timeScale = 1f;
            ChangeState(GameState.Playing, true);

            AppLogger.Log(name, "Successfully reloaded game.");
        }

        private void HandleBallDropped()
        {
            _remainingLives--;

            if (_remainingLives <= 0)
            {
                _remainingLives = 0;
                AppLogger.Log(name, $"Remaining lives: {_remainingLives}.");
                GameOver();
                return;
            }

            AppLogger.Log(name, $"Remaining lives: {_remainingLives}.");
        }

        private void HandleBrickDestroyed(int score)
        {
            _currentScore += score;

            AppLogger.Log(name, $"Current score: {_currentScore}.");
        }

        private void HandleLevelCompleted()
        {
            Time.timeScale = 0f;
            AppLogger.Log(name, "Level completed.");
            ChangeState(GameState.LevelCompleted, false);
        }

        #endregion
    }
}