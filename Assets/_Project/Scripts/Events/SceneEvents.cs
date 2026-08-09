using System;

namespace _Project.Scripts.Events
{
    public static class SceneEvents
    {
        public static Action<string> OnRequestLoadScene;
        public static Action OnRequestLoadMainMenu;
        public static Action OnRequestLoadGameplay;
    }
}