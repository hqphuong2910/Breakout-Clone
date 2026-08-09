using System;
using _Project.Scripts.Enums;

namespace _Project.Scripts.Events
{
    public static class UIEvents
    {
        public static Action<ScreenType> OnOpenScreen;
        public static Action OnCloseTopScreen;
        public static Action OnCloseAllScreens;
    }
}