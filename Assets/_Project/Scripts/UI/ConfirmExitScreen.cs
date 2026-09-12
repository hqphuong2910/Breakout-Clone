using _Project.Scripts.Patterns;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ConfirmExitScreen : Singleton<ConfirmExitScreen>
    {
        [SerializeField] private Button confirmButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!confirmButton) return;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(Quit);
        }

        private static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}