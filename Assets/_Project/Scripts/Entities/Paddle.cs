using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Paddle : MyBehaviour
    {
        [Header("Control Configuration")] [SerializeField]
        private ControlConfigSO controlConfig;

        [Header("Movement Setting(s)")] [SerializeField]
        private float moveSpeed = 4f;

        [Header("Boundaries")] [SerializeField]
        private float maxX = 2.085f;

        private float _moveInput;
        private float _pointerTargetX;

        private Rigidbody2D _rb;

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            var moveDirection = new Vector2();
            switch (controlConfig.controlType)
            {
                case ControlType.KeyboardOrGamepad:
                    var moveDelta = _moveInput * moveSpeed * Time.fixedDeltaTime;
                    var nextX = transform.position.x + moveDelta;
                    nextX = Mathf.Clamp(nextX, -maxX, maxX);
                    moveDirection = new Vector2(nextX, transform.position.y);
                    break;
                case ControlType.MouseOrTouchscreen:
                    _pointerTargetX = Mathf.Clamp(_pointerTargetX, -maxX, maxX);
                    moveDirection = new Vector2(_pointerTargetX, transform.position.y);
                    break;
            }

            _rb.MovePosition(moveDirection);
        }

        #region INITIALIZATION

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadRigidbody();
        }

        private void LoadRigidbody()
        {
            if (_rb) return;
            _rb = GetComponent<Rigidbody2D>();
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

            AppLogger.Log(name, $"Successfully loaded {nameof(Rigidbody2D)} component.");
        }

        #endregion

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            InputEvents.OnMoveByKeys += GetKeyInputValue;
            InputEvents.OnMoveByPointer += GetPointerPosition;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            InputEvents.OnMoveByKeys -= GetKeyInputValue;
            InputEvents.OnMoveByPointer -= GetPointerPosition;
        }

        #endregion

        #region INPUT_READERS

        private void GetKeyInputValue(float value)
        {
            if (controlConfig.controlType != ControlType.KeyboardOrGamepad) return;
            _moveInput = value;
        }

        private void GetPointerPosition(Vector2 screenPos)
        {
            if (controlConfig.controlType != ControlType.MouseOrTouchscreen) return;
            if (!Camera.main) return;
            var posZ = Mathf.Abs(Camera.main.transform.position.z);
            var newPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, posZ));
            _pointerTargetX = newPos.x;
        }

        #endregion
    }
}