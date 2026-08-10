using _Project.Scripts.Core;
using _Project.Scripts.Events;
using _Project.Scripts.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MyBehaviour
    {
        [Header("Launch Setting(s)")] [SerializeField]
        private float ballSpeed = 6f;

        [SerializeField] private float maxXDirection = 1f;

        [Header("Follow Setting(s)")] [SerializeField]
        private Vector2 offsetWithPaddle = new(0f, 0.3f);

        [Header("Bounce Setting(s)")] [SerializeField]
        private float bounceMultiplier = 1f;

        private bool _isLaunched;
        private Transform _paddle;
        private Rigidbody2D _rb;

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
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.gravityScale = 0;
            _rb.linearDamping = 0;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            AppLogger.Log(name, $"Successfully loaded {nameof(Rigidbody2D)} component.");
        }

        #endregion

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            GameEvents.OnPaddleReady += GetPaddleTransform;
            InputEvents.OnLaunch += LaunchBall;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            GameEvents.OnPaddleReady -= GetPaddleTransform;
            InputEvents.OnLaunch -= LaunchBall;
        }

        #endregion

        #region FOLLOW_PADDLE

        private void LateUpdate()
        {
            FollowPaddle();
        }

        private void GetPaddleTransform(Transform paddle)
        {
            _paddle = paddle;
        }

        private void FollowPaddle()
        {
            if (_isLaunched) return;
            transform.position = _paddle.position + (Vector3)offsetWithPaddle;
        }

        #endregion

        #region BALL_PHYSICS

        private void FixedUpdate()
        {
            if (!_isLaunched) return;
            _rb.linearVelocity = _rb.linearVelocity.normalized * ballSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Paddle paddle))
            {
                var contactPoint = other.GetContact(0).point;
                var offsetY = contactPoint.y - paddle.transform.position.y;
                if (offsetY < -0.05f) return;
                var offsetX = contactPoint.x - paddle.transform.position.x;
                var newDirection = new Vector2(offsetX * bounceMultiplier, 1f).normalized;
                _rb.linearVelocity = newDirection * ballSpeed;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("DeadZone"))
            {
                GameEvents.OnBallDropped?.Invoke();
                AppLogger.Log(name, $"Dropped into {other.name}.");
                ResetBall();
            }
        }

        private void LaunchBall()
        {
            if (_isLaunched) return;
            var randomX = Random.Range(-maxXDirection, maxXDirection);
            var direction = new Vector2(randomX, 1f).normalized;
            _rb.linearVelocity = direction * ballSpeed;
            _isLaunched = true;

            AppLogger.Log(name, "Launched.");
        }

        private void ResetBall()
        {
            _isLaunched = false;
            _rb.linearVelocity = Vector2.zero;

            AppLogger.Log(name, "Reset.");
        }

        #endregion
    }
}