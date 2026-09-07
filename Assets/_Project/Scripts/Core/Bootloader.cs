using System.Collections;
using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Enums;
using _Project.Scripts.Events;
using _Project.Scripts.Managers;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;

namespace _Project.Scripts.Core
{
    [DefaultExecutionOrder(int.MinValue)]
    public class Bootloader : Singleton<Bootloader>
    {
        [Header("Dependencies")] [SerializeField]
        private GameConfigSO gameConfig;

        [SerializeField] private ControlConfigSO controlConfig;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        protected override void LoadDependencies()
        {
            base.LoadDependencies();

            ApplyGameConfig();
            StartCoroutine(InitializeSystem());
        }

        private void ApplyGameConfig()
        {
            if (!gameConfig)
            {
                AppLogger.LogError(this,
                    $"Missing {nameof(gameConfig)} reference. " +
                    "Please drag and drop it in the inspector.");
                return;
            }

            AppLogger.LogEnabled = gameConfig.enableLogs;

            var settings = SaveSystem.Load<SettingsData>(SysConst.SettingsFileName);

            gameConfig.targetFPS = settings.targetFPS;
            gameConfig.vSync = settings.vSync;
            if (controlConfig) controlConfig.controlMethods = settings.controlMethods;

            Application.targetFrameRate = settings.targetFPS switch
            {
                FPSLimit.Unlimited => -1,
                FPSLimit.Limit30 => 30,
                FPSLimit.Limit60 => 60,
                FPSLimit.Limit90 => 90,
                FPSLimit.Limit120 => 120,
                FPSLimit.Limit144 => 144,
                FPSLimit.Limit165 => 165,
                FPSLimit.Limit180 => 180,
                FPSLimit.Limit240 => 240,
                _ => 60
            };
            QualitySettings.vSyncCount = settings.vSync ? 1 : 0;

            AppLogger.Log(this,
                $"Loading {gameConfig.gameName}: " +
                $"Version: {gameConfig.gameVersion}");
        }

        private IEnumerator InitializeSystem()
        {
            AppLogger.Log(this, "Initializing backend systems...");
            GameEvents.OnInitializing?.Invoke();
            yield return null;

            var settings = SaveSystem.Load<SettingsData>(SysConst.SettingsFileName);
            if (!AudioManager.Instance) yield break;
            AudioManager.Instance.SetMasterVolume(settings.masterVolume);
            AudioManager.Instance.SetBGMVolume(settings.bgmVolume);
            AudioManager.Instance.SetSFXVolume(settings.sfxVolume);

            AppLogger.Log(this, "Backend systems are ready.");
            SystemEvents.OnSystemReady?.Invoke();
        }
    }
}