using System;
using System.Collections.Generic;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("UI Configurations")]
        [Tooltip("Attach the screen objects and set their corresponding screen type here.")]
        [SerializeField]
        private List<ScreenMapping> screenMappings;

        [SerializeField] private ScreenType defaultScreen = ScreenType.None;

        private readonly Dictionary<ScreenType, GameObject> _screenDict = new();
        private readonly Stack<GameObject> _screenStack = new();

        [Serializable]
        public struct ScreenMapping
        {
            public ScreenType screenType;
            public GameObject screenObject;
        }

        #region INITIALIZATION

        protected override void LoadComponents()
        {
            base.LoadComponents();

            if (screenMappings is not { Count: > 0 }) return;
            foreach (var mapping in screenMappings)
            {
                if (mapping.screenType == ScreenType.None || !mapping.screenObject)
                {
                    AppLogger.LogError(this, "Invalid screen mapping found.");
                    return;
                }

                mapping.screenObject.SetActive(false);
                _screenDict.Add(mapping.screenType, mapping.screenObject);
            }
        }

        protected override void LoadDependencies()
        {
            base.LoadDependencies();

            if (defaultScreen != ScreenType.None) OpenScreen(defaultScreen);
        }

        #endregion

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            UIEvents.OnOpenScreen += OpenScreen;
            UIEvents.OnCloseTopScreen += CloseTopScreen;
            UIEvents.OnCloseAllScreens += CloseAllScreens;
            GameEvents.OnGameStateChanged += HandleGameStateChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            UIEvents.OnOpenScreen -= OpenScreen;
            UIEvents.OnCloseTopScreen -= CloseTopScreen;
            UIEvents.OnCloseAllScreens -= CloseAllScreens;
            GameEvents.OnGameStateChanged -= HandleGameStateChanged;
        }

        #endregion

        #region SCREEN_HANDLERS

        private void OpenScreen(ScreenType type)
        {
            if (type == ScreenType.None) return;

            if (!_screenDict.TryGetValue(type, out var targetScreen))
            {
                AppLogger.LogWarning(this, $"Screen {type} is not registered in {name}.");
                return;
            }

            if (_screenStack.Count > 0) _screenStack.Peek().SetActive(false);

            targetScreen.SetActive(true);
            _screenStack.Push(targetScreen);

            AppLogger.Log(this, $"Opened screen: {type}.");
        }

        private void CloseTopScreen()
        {
            if (_screenStack.Count <= 0)
            {
                AppLogger.LogWarning(this, "No screens to close.");
                return;
            }

            var topScreen = _screenStack.Pop();
            topScreen.SetActive(false);

            if (_screenStack.Count > 0) _screenStack.Peek().SetActive(true);
        }

        private void CloseAllScreens()
        {
            while (_screenStack.Count > 0) _screenStack.Pop().SetActive(false);
        }

        private void HandleGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Initializing:
                    OpenScreen(ScreenType.LoadingScreen);
                    break;
                case GameState.Started:
                    CloseAllScreens();
                    OpenScreen(ScreenType.HUD);
                    break;
                case GameState.Paused:
                    OpenScreen(ScreenType.PauseMenu);
                    break;
                case GameState.LevelCompleted:
                    break;
                case GameState.GameOver:
                    OpenScreen(ScreenType.GameOver);
                    break;
                default:
                    AppLogger.LogError(this, $"No suitable screens found for current game state: {state}.");
                    break;
            }
        }

        #endregion
    }
}