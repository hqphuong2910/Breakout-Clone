using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using _Project.Settings.Input;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private GameInputActions _inputActions;

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadInputActions();
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _inputActions.Enable();
            SubscribeUIInput();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _inputActions.Disable();
            UnsubscribeUIInput();
        }

        private void LoadInputActions()
        {
            if (_inputActions != null) return;
            _inputActions = new GameInputActions();
            AppLogger.Log(name, $"Successfully loaded {nameof(GameInputActions)}.");
        }

        private void SubscribeUIInput()
        {
            _inputActions.UI.Cancel.performed += HandleCancel;
        }

        private void UnsubscribeUIInput()
        {
            _inputActions.UI.Cancel.performed -= HandleCancel;
        }

        private static void HandleCancel(InputAction.CallbackContext context)
        {
            InputEvents.OnCancel?.Invoke();
        }
    }
}