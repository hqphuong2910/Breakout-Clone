using _Project.Scripts.Events;
using _Project.Scripts.Managers;
using _Project.Scripts.Patterns;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class GameplayHUD : Singleton<GameplayHUD>
    {
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text scoreText;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (GameManager.Instance)
                UpdateHUD(
                    GameManager.Instance.RemainingLives,
                    GameManager.Instance.CurrentScore
                );
        }

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            GameEvents.OnLivesChanged += UpdateLivesText;
            GameEvents.OnScoreChanged += UpdateScoreText;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            GameEvents.OnLivesChanged -= UpdateLivesText;
            GameEvents.OnScoreChanged -= UpdateScoreText;
        }

        #endregion

        #region HUD_HANDLERS

        private void UpdateHUD(int lives, int score)
        {
            UpdateLivesText(lives);
            UpdateScoreText(score);
        }

        private void UpdateLivesText(int lives)
        {
            if (!livesText) return;
            if (lives < 0) lives = 0;
            livesText.text = $"LIVES: {lives}";
        }

        private void UpdateScoreText(int score)
        {
            if (!scoreText) return;
            if (score < 0) score = 0;
            scoreText.text = $"SCORE: {score}";
        }

        #endregion
    }
}