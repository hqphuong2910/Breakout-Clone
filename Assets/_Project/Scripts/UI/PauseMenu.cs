using _Project.Scripts.Managers;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
        [SerializeField] private Button resumeButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!resumeButton) return;
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
        }

        private static void ResumeGame()
        {
            GameManager.Instance.ResumeGame();
        }
    }
}