using System.Collections;
using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.Events;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class IntroController : MyBehaviour
    {
        [Header("Dependencies")] [SerializeField]
        private GameConfigSO gameConfig;

        [Header("Intro Setting(s)")] [SerializeField]
        private float introDuration = 2.5f;

        [SerializeField] private bool allowSkip;

        private bool _isIntroFinished;
        private bool _isSystemReady;
        private bool _isTransitioning;

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            SystemEvents.OnSystemReady += HandleSystemReady;
            InputEvents.OnCancel += SkipIntro;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            SystemEvents.OnSystemReady -= HandleSystemReady;
            InputEvents.OnCancel -= SkipIntro;
        }

        protected override void LoadDependencies()
        {
            base.LoadDependencies();
            StartCoroutine(PlayIntroRoutine());
        }

        private IEnumerator PlayIntroRoutine()
        {
            AppLogger.Log(this, $"Playing intro for {introDuration} seconds...");
            yield return new WaitForSeconds(introDuration);

            if (!_isIntroFinished) FinishIntro();
        }

        private void HandleSystemReady()
        {
            _isSystemReady = true;
            AppLogger.Log(this, "System is ready.");
            TryTransition();
        }

        private void FinishIntro()
        {
            _isIntroFinished = true;
            AppLogger.Log(this, "Intro finished.");

            if (!_isSystemReady) AppLogger.LogWarning(this, "Waiting for backend to finish loading...");
            // TODO: Put a text panel "Loading..." or something else here.
            TryTransition();
        }

        private void TryTransition()
        {
            if (!_isSystemReady || !_isIntroFinished || _isTransitioning) return;
            _isTransitioning = true;
            StopAllCoroutines();

            if (gameConfig && gameConfig.bypassMainMenu)
            {
                AppLogger.Log(this, "Requesting Gameplay scene.");
                SceneEvents.OnRequestLoadGameplay?.Invoke();
            }
            else
            {
                AppLogger.Log(this, "Requesting Main Menu scene.");
                SceneEvents.OnRequestLoadMainMenu?.Invoke();
            }
        }

        private void SkipIntro()
        {
            if (!allowSkip || _isIntroFinished) return;
            AppLogger.Log(this, "Intro skipped via user input.");
            FinishIntro();
        }
    }
}