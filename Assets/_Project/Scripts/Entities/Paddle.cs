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

        #region INITIALIZATION

        protected override void Start()
        {
            base.Start();

            GameEvents.OnPaddleReady?.Invoke(transform);
        }

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

            AppLogger.Log(this, $"Successfully loaded {nameof(Rigidbody2D)} component.");
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
            if (controlConfig.controlMethods != ControlMethods.KeyboardOrGamepad) return;
            _moveInput = value;
        }

        private void GetPointerPosition(Vector2 screenPos)
        {
            if (controlConfig.controlMethods != ControlMethods.MouseOrTouchscreen) return;
            if (!Camera.main) return;
            var posZ = Mathf.Abs(Camera.main.transform.position.z);
            var newPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, posZ));
            _pointerTargetX = newPos.x;
        }

        #endregion

        #region PADDLE_PHYSICS

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 moveDirection;
            switch (controlConfig.controlMethods)
            {
                case ControlMethods.KeyboardOrGamepad:
                    var moveDelta = _moveInput * moveSpeed * Time.fixedDeltaTime;
                    var nextX = transform.position.x + moveDelta;
                    nextX = Mathf.Clamp(nextX, -maxX, maxX);
                    moveDirection = new Vector2(nextX, transform.position.y);
                    break;
                case ControlMethods.MouseOrTouchscreen:
                    _pointerTargetX = Mathf.Clamp(_pointerTargetX, -maxX, maxX);
                    moveDirection = new Vector2(_pointerTargetX, transform.position.y);
                    break;
                default:
                    moveDirection = Vector2.zero;
                    break;
            }

            _rb.MovePosition(moveDirection);
        }

        #endregion
    }
}