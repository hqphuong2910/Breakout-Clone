using System.Collections.Generic;
using UnityEngine;

namespace _Project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Breakout/Brick Data", fileName = "NewBrickData")]
    public class BrickDataSO : ScriptableObject
    {
        [Header("Visual")]
        [Tooltip(
            "List of brick sprites. " +
            "Element 0 is the original state, " +
            "and the following elements are states that get progressively more cracked.")]
        public List<Sprite> brickSprites = new();

        public Color brickColor = Color.white;

        [Tooltip("The VFX appear when this brick gets hit.")]
        public GameObject hitVFX;

        [Header("Audio")] [Tooltip("The sound that will play when this brick gets hit.")]
        public AudioClip hitSFX;

        [Tooltip("The sound that will play when this brick is broken.")]
        public AudioClip destroyedSFX;

        [Header("Data")] [Tooltip("If this field is checked, all the following fields will be ignored.")]
        public bool isUnbreakable;

        [Tooltip("Maximum number of hits to destroy this brick.")]
        public int maxHits = 1;

        [Tooltip("The score you will get when this brick is broken.")]
        public int scoreValue = 10;

        [Tooltip("The power-up drop rate of this brick, from 0 to 100 percent.")] [Range(0f, 100f)]
        public float powerUpDropRate;
    }
}