using System;
using UnityEngine;

namespace _Project.Scripts.Events
{
    public static class InputEvents
    {
        // UI events
        public static Action OnCancel;

        // Gameplay events
        public static Action<float> OnMoveByKeys;
        public static Action<Vector2> OnMoveByPointer;
    }
}