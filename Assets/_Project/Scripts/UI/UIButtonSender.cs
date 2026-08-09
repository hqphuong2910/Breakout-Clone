using _Project.Scripts.Core;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonSender : MyBehaviour
    {
        public enum ActionType
        {
            OpenScreen,
            CloseTopScreen,
            CloseAllScreens
        }

        [SerializeField] private ActionType actionType;

        [Tooltip("Only works if ActionType is OpenScreen.")] [SerializeField]
        private ScreenType targetScreen;

        private Button _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            switch (actionType)
            {
                case ActionType.OpenScreen:
                    UIEvents.OnOpenScreen?.Invoke(targetScreen);
                    break;
                case ActionType.CloseTopScreen:
                    UIEvents.OnCloseTopScreen?.Invoke();
                    break;
                case ActionType.CloseAllScreens:
                    UIEvents.OnCloseAllScreens?.Invoke();
                    break;
            }
        }
    }
}