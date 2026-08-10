using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Breakout/Game Configuration", fileName = "GameConfig")]
    public class GameConfigSO : ScriptableObject
    {
        [Header("App Info")] public string appName = "Breakout";
        public string appVersion = "0.1.0";

        [Header("Performance Setting(s)")] public FPSLimit targetFPS = FPSLimit.Limit60;
        public bool vSync = true;

        [Header("Scene Routing")] public string mainMenuSceneName = "01_MainMenu";
        public string gameplaySceneName = "02_Gameplay";

        [Header("Debug Setting(s)")] public bool enableLogs = true;
        public bool bypassMainMenu;
    }
}