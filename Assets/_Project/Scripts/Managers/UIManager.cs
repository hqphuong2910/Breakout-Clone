using System;
using System.Collections.Generic;
using System.Linq;
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
        [Tooltip("Drag and drop the screen objects that match the screen types here.")]
        [SerializeField]
        private List<ScreenMapping> screenMappings;

        [SerializeField] private ScreenType defaultScreen = ScreenType.None;

        private readonly Dictionary<ScreenType, GameObject> _screenDict = new();
        private readonly Stack<GameObject> _screenStack = new();

        protected override void LoadComponents()
        {
            base.LoadComponents();

            foreach (var mapping in screenMappings.Where(mappings => mappings.screenObject))
            {
                mapping.screenObject.SetActive(false);
                _screenDict.Add(mapping.screenType, mapping.screenObject);
            }
        }

        protected override void LoadDependencies()
        {
            base.LoadDependencies();

            if (defaultScreen != ScreenType.None) OpenScreen(defaultScreen);
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            UIEvents.OnOpenScreen += OpenScreen;
            UIEvents.OnCloseTopScreen += CloseTopScreen;
            UIEvents.OnCloseAllScreens += CloseAllScreens;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            UIEvents.OnOpenScreen -= OpenScreen;
            UIEvents.OnCloseTopScreen -= CloseTopScreen;
            UIEvents.OnCloseAllScreens -= CloseAllScreens;
        }

        private void OpenScreen(ScreenType type)
        {
            if (!_screenDict.TryGetValue(type, out var targetScreen))
            {
                AppLogger.LogWarning(name, $"Screen {type} is not registered in {name}.");
                return;
            }

            if (_screenStack.Count > 0) _screenStack.Peek().SetActive(false);

            targetScreen.SetActive(true);
            _screenStack.Push(targetScreen);

            AppLogger.Log(name, $"Opened screen: {type}.");
        }

        private void CloseTopScreen()
        {
            if (_screenStack.Count <= 0)
            {
                AppLogger.LogWarning(name, "No screens to close.");
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

        [Serializable]
        public struct ScreenMapping
        {
            public ScreenType screenType;
            public GameObject screenObject;
        }
    }
}