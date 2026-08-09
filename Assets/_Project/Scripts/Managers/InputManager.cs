using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using _Project.Settings.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        private GameInputActions _inputActions;

        #region INITIALIZATION

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadInputActions();
        }

        private void LoadInputActions()
        {
            if (_inputActions != null) return;
            _inputActions = new GameInputActions();
            AppLogger.Log(name, $"Successfully loaded {nameof(GameInputActions)}.");
        }

        #endregion

        #region EVENT_HANDLERS

        #region GLOBAL

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _inputActions.Enable();
            SubscribeUIInput();
            SubscribeGameplayInput();
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _inputActions.Disable();
            UnsubscribeUIInput();
            UnsubscribeGameplayInput();
        }

        #endregion

        #region UI

        private void SubscribeUIInput()
        {
            _inputActions.UI.Cancel.performed += PerformCancel;
        }

        private void UnsubscribeUIInput()
        {
            _inputActions.UI.Cancel.performed -= PerformCancel;
        }

        #endregion

        #region GAMEPLAY

        private void SubscribeGameplayInput()
        {
            _inputActions.Gameplay.MoveByKeys.performed += PerformMoveByKeys;
            _inputActions.Gameplay.MoveByKeys.canceled += CancelMoveByKeys;
            _inputActions.Gameplay.MoveByPointer.performed += PerformMoveByPointer;
            _inputActions.Gameplay.Launch.performed += PerformLaunch;
        }

        private void UnsubscribeGameplayInput()
        {
            _inputActions.Gameplay.MoveByKeys.performed -= PerformMoveByKeys;
            _inputActions.Gameplay.MoveByKeys.canceled -= CancelMoveByKeys;
            _inputActions.Gameplay.MoveByPointer.performed -= PerformMoveByPointer;
            _inputActions.Gameplay.Launch.performed -= PerformLaunch;
        }

        #endregion

        #endregion

        #region INPUT_HANDLERS

        #region UI

        private static void PerformCancel(InputAction.CallbackContext context)
        {
            InputEvents.OnCancel?.Invoke();
        }

        #endregion

        #region GAMEPLAY

        private static void PerformMoveByKeys(InputAction.CallbackContext context)
        {
            InputEvents.OnMoveByKeys?.Invoke(context.ReadValue<float>());
        }

        private static void CancelMoveByKeys(InputAction.CallbackContext context)
        {
            InputEvents.OnMoveByKeys?.Invoke(0f);
        }

        private static void PerformMoveByPointer(InputAction.CallbackContext context)
        {
            InputEvents.OnMoveByPointer?.Invoke(context.ReadValue<Vector2>());
        }

        private static void PerformLaunch(InputAction.CallbackContext context)
        {
            InputEvents.OnLaunch?.Invoke();
        }

        #endregion

        #endregion
    }
}