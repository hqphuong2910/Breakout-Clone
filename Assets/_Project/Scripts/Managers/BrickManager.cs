using System.Collections.Generic;
using _Project.Scripts.Entities;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using _Project.Scripts.Extensions;
using _Project.Scripts.Patterns;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class BrickManager : Singleton<BrickManager>
    {
        [Header("Prefab References")]
        [Tooltip("The brick prefabs. All prefabs must have the same scale for an even grid.")]
        [SerializeField]
        private List<GameObject> brickPrefabs;

        [Header("Grid Setting(s)")] [SerializeField]
        private float columns = 9;

        [SerializeField] private int rows = 9;
        [SerializeField] private float startPosY = 4.7f;
        [SerializeField] private float gapX = 0.05f, gapY = 0.05f;

        private int _breakableBrickCount;

        protected override void Start()
        {
            base.Start();

            if (GameManager.Instance.CurrentState == GameState.Started)
                GenerateBricks(GameManager.Instance.CurrentState);
        }

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            GameEvents.OnGameStateChanged += GenerateBricks;
            GameEvents.OnBrickDestroyed += HandleBrickDestroy;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            GameEvents.OnGameStateChanged -= GenerateBricks;
            GameEvents.OnBrickDestroyed -= HandleBrickDestroy;
        }

        #endregion

        #region BRICK_HANDLERS

        private void GenerateBricks(GameState currentState)
        {
            if (currentState != GameState.Started) return;

            ClearBricks();
            _breakableBrickCount = 0;

            if (brickPrefabs is not { Count: > 0 }) return;

            var sampleBrick = brickPrefabs[0];

            var brickWidth = sampleBrick.transform.localScale.x;
            var brickHeight = sampleBrick.transform.localScale.y;

            var strideX = brickWidth + gapX;
            var strideY = brickHeight + gapY;

            var offsetX = (columns - 1) * strideX / 2f;

            for (var row = 0; row < rows; row++)
            for (var col = 0; col < columns; col++)
            {
                var randomIndex = Random.Range(0, brickPrefabs.Count);
                var brick = brickPrefabs[randomIndex];
                if (!brick) continue;

                var posX = col * strideX - offsetX;
                var posY = startPosY - row * strideY;
                var spawnPos = new Vector3(posX, posY, 0f);

                var spawnedObj = brick.Spawn(spawnPos, Quaternion.identity);

                if (spawnedObj.TryGetComponent<Brick>(out var brickComp) && !brickComp.IsUnbreakable)
                    _breakableBrickCount++;
            }
        }

        private static void ClearBricks()
        {
            var activeBricks = FindObjectsByType<Brick>(FindObjectsSortMode.None);
            foreach (var brick in activeBricks) brick.gameObject.Despawn();
        }

        private void HandleBrickDestroy(int score)
        {
            _breakableBrickCount--;

            if (_breakableBrickCount <= 0) GameEvents.OnLevelCompleted?.Invoke();
        }

        #endregion
    }
}