using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Breakout/Control Configuration", fileName = "ControlConfig")]
    public class ControlConfigSO : ScriptableObject
    {
        public ControlMethods controlMethods = ControlMethods.KeyboardOrGamepad;
    }
}