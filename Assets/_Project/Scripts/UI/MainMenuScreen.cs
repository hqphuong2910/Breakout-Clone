using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class MainMenuScreen : Singleton<MainMenuScreen>
    {
        [SerializeField] private Button playButton;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!playButton) return;
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(Play);
        }

        private static void Play()
        {
            SceneEvents.OnRequestLoadGameplay?.Invoke();
        }
    }
}