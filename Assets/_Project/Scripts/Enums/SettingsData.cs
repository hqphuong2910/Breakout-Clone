using System;
using UnityEngine;

namespace _Project.Scripts.Enums
{
    [Serializable]
    public class SettingsData
    {
        [Header("Sound Setting(s)")] public float masterVolume = 1.0f;

        public float bgmVolume = 1.0f;
        public float sfxVolume = 1.0f;

        [Header("Performance Setting(s)")] public bool vSync = true;

        public FPSLimit targetFPS = FPSLimit.Limit60;

        [Header("Control Setting(s)")] public ControlMethods controlMethods = ControlMethods.KeyboardOrGamepad;
    }
}