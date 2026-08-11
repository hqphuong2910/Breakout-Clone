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
        public int CurrentScore { get; private set; }
        public int RemainingLives { get; private set; }
        public GameState CurrentState { get; private set; }

        protected override void Start()
        {
            base.Start();

            ResetGameData();
            ChangeState(GameState.Initializing, false);
        }

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            GameEvents.OnGameStarted += HandleGameStarted;
            GameEvents.OnBrickDestroyed += AddScore;
            GameEvents.OnBallDropped += RemoveLives;
            GameEvents.OnLevelCompleted += HandleLevelCompleted;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            GameEvents.OnGameStarted -= HandleGameStarted;
            GameEvents.OnBrickDestroyed -= AddScore;
            GameEvents.OnBallDropped -= RemoveLives;
            GameEvents.OnLevelCompleted -= HandleLevelCompleted;
        }

        #endregion

        #region GAME_FLOW_HANDLERS

        private void ResetGameData()
        {
            RemainingLives = maxLives;
            AppLogger.Log(this, $"Remaining lives: {RemainingLives}.");
            GameEvents.OnLivesChanged?.Invoke(RemainingLives);

            CurrentScore = 0;
            AppLogger.Log(this, $"Current score: {CurrentScore}.");
            GameEvents.OnScoreChanged?.Invoke(CurrentScore);

            AppLogger.Log(this, "Reset game data.");
        }

        private void RemoveLives()
        {
            RemainingLives--;

            if (RemainingLives <= 0)
            {
                RemainingLives = 0;
                AppLogger.Log(this, $"Remaining lives: {RemainingLives}.");
                GameOver();
                return;
            }

            AppLogger.Log(this, $"Remaining lives: {RemainingLives}.");

            GameEvents.OnLivesChanged?.Invoke(RemainingLives);
        }

        private void AddScore(int score)
        {
            CurrentScore += score;

            AppLogger.Log(this, $"Current score: {CurrentScore}.");

            GameEvents.OnScoreChanged?.Invoke(CurrentScore);
        }

        public void ReloadGame()
        {
            ResetGameData();
            Time.timeScale = 1f;
            ChangeState(GameState.Started, true);

            AppLogger.Log(this, "Successfully reloaded game.");
        }

        private void PauseGame()
        {
            if (CurrentState != GameState.Started) return;

            Time.timeScale = 0f;
            ChangeState(GameState.Paused, false);
            AppLogger.Log(this, "Game paused.");
        }

        private void ResumeGame()
        {
            if (CurrentState != GameState.Paused) return;

            Time.timeScale = 1f;
            ChangeState(GameState.Started, true);
            AppLogger.Log(this, "Game resumed.");
        }

        private void GameOver()
        {
            Time.timeScale = 0f;

            ChangeState(GameState.GameOver, false);
            AppLogger.Log(this, "Game over.");
            GameEvents.OnGameOver?.Invoke();
        }

        #endregion

        #region GAME_STATE_HANDLERS

        private void ChangeState(GameState newState, bool force)
        {
            if (!force && CurrentState == newState) return;

            CurrentState = newState;
            GameEvents.OnGameStateChanged?.Invoke(CurrentState);
        }

        private void HandleInitializing()
        {
            ChangeState(GameState.Initializing, false);
        }

        private void HandleGameStarted()
        {
            ReloadGame();
        }

        private void HandleGamePaused()
        {
            ChangeState(GameState.Paused, false);
        }

        private void HandleLevelCompleted()
        {
            Time.timeScale = 0f;
            AppLogger.Log(this, "Level completed.");
            ChangeState(GameState.LevelCompleted, false);
        }

        private void HandleGameOver()
        {
            ChangeState(GameState.GameOver, false);
        }

        #endregion
    }
}