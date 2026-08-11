using System.Collections;
using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Events;
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

            Application.targetFrameRate = (int)gameConfig.targetFPS;
            QualitySettings.vSyncCount = gameConfig.vSync ? 1 : 0;

            AppLogger.Log(this,
                $"Loading {gameConfig.gameName}: " +
                $"Version: {gameConfig.gameVersion}");
        }

        private IEnumerator InitializeSystem()
        {
            AppLogger.Log(this, "Initializing backend systems...");
            GameEvents.OnInitializing?.Invoke();
            // TODO: Delete the line below and implement actual system initialization logic.
            yield return new WaitForSeconds(0.5f);
            AppLogger.Log(this, "Backend systems are ready.");
            SystemEvents.OnSystemReady?.Invoke();
        }
    }
}