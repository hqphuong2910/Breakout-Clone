using _Project.Scripts.Events;
using _Project.Scripts.Managers;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameOverScreen : Singleton<GameOverScreen>
    {
        [SerializeField] private Button restartButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!restartButton) return;
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(Restart);
        }

        private static void Restart()
        {
            if (!GameManager.Instance) return;
            GameManager.Instance.ReloadGame();
        }
    }
}