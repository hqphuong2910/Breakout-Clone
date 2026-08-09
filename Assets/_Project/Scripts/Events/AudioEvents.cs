using System;
using UnityEngine;

namespace _Project.Scripts.Events
{
    public static class AudioEvents
    {
        public static Action<AudioClip, bool> OnPlayBGM;
        public static Action OnStopBGM;
        public static Action<AudioClip> OnPlaySFX;
    }
}