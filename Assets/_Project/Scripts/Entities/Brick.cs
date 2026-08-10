using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Core;
using _Project.Scripts.Events;
using _Project.Scripts.Extensions;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Brick : MyBehaviour
    {
        [SerializeField] private BrickDataSO brickData;

        private int _currentHp;

        private SpriteRenderer _renderer;

        public bool IsUnbreakable => brickData && brickData.isUnbreakable;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!brickData) return;
            if (brickData.isUnbreakable) return;
            if (!other.gameObject.TryGetComponent<Ball>(out var ball)) return;
            _currentHp--;
            if (_currentHp <= 0)
                Die();
            else
                UpdateCrackedSprites();
        }

        private void ResetBrick()
        {
            if (!brickData)
            {
                AppLogger.LogError(name, $"No {nameof(brickData)} found.");
                return;
            }

            if (brickData.brickSprites is { Count: > 0 }) _renderer.sprite = brickData.brickSprites[0];
            _renderer.color = brickData.brickColor;
            _currentHp = brickData.maxHits;
        }

        private void Die()
        {
            gameObject.Despawn();
            GameEvents.OnBrickDestroyed?.Invoke(brickData.scoreValue);
        }

        private void UpdateCrackedSprites()
        {
            if (!brickData) return;
            if (brickData.brickSprites is not { Count: > 0 }) return;
            var index = brickData.maxHits - _currentHp;
            if (index >= brickData.brickSprites.Count) index = brickData.brickSprites.Count - 1;
            _renderer.sprite = brickData.brickSprites[index];
        }

        #region INITIALIZATION

        protected override void OnEnable()
        {
            base.OnEnable();

            ResetBrick();
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadRenderer();
        }

        private void LoadRenderer()
        {
            if (_renderer) return;
            _renderer = GetComponent<SpriteRenderer>();

            AppLogger.Log(name, $"Successfully loaded {nameof(SpriteRenderer)} component.");
        }

        #endregion
    }
}