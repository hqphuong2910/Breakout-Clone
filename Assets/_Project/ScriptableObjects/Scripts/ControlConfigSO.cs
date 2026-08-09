using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Breakout/ControlConfig", fileName = "ControlConfig")]
    public class ControlConfigSO : ScriptableObject
    {
        public ControlType controlType = ControlType.KeyboardOrGamepad;
    }
}