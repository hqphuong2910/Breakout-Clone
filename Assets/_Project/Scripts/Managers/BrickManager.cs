using System.Collections.Generic;
using _Project.Scripts.Extensions;
using _Project.Scripts.Patterns;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class BrickManager : Singleton<BrickManager>
    {
        [Header("Prefab References")] [SerializeField]
        private List<GameObject> brickPrefabs;

        [Header("Grid Setting(s)")] [SerializeField]
        private float columns = 9;

        [SerializeField] private int rows = 9;
        [SerializeField] private float startPosY = 4.7f;
        [SerializeField] private float gapX = 0.05f, gapY = 0.05f;

        protected override void Start()
        {
            base.Start();

            GenerateBricks();
        }

        public void GenerateBricks()
        {
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

                brick.Spawn(spawnPos, Quaternion.identity);
            }
        }
    }
}