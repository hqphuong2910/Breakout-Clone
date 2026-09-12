using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ConfirmBackScreen : Singleton<ConfirmBackScreen>
    {
        [SerializeField] private Button confirmButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!confirmButton) return;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(BackToMainMenu);
        }

        private static void BackToMainMenu()
        {
            Time.timeScale = 1f;
            SceneEvents.OnRequestLoadMainMenu?.Invoke();
        }
    }
}