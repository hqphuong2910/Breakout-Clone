using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.ScriptableObjects.Scripts
{
    [CreateAssetMenu(menuName = "Breakout/Game Configuration", fileName = "GameConfig")]
    public class GameConfigSO : ScriptableObject
    {
        [Header("Game Info")] public string gameName = "Breakout";
        public string gameVersion;

        [Header("Performance Setting(s)")] public FPSLimit targetFPS = FPSLimit.Limit60;
        public bool vSync = true;

        [Header("Scene Routing")] public string mainMenuSceneName = "01_MainMenu";
        public string gameplaySceneName = "02_Gameplay";

        [Header("Debug Setting(s)")] public bool enableLogs = true;
        public bool bypassMainMenu;
    }
}