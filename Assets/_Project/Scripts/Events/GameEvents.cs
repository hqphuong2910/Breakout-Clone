using System;
using UnityEngine;

namespace _Project.Scripts.Events
{
    public static class GameEvents
    {
        public static Action<Transform> OnPaddleReady;
        public static Action OnBallDropped;
        public static Action OnBrickDestroyed;
    }
}