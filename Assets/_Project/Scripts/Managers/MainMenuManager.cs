using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        public void Play()
        {
            SceneEvents.OnRequestLoadGameplay.Invoke();
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}