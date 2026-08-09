using _Project.Scripts.Core;

namespace _Project.Scripts.Patterns
{
    public class Singleton<T> : MyBehaviour where T : MyBehaviour
    {
        public static T Instance { get; private set; }

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadInstance();
        }

        private void LoadInstance()
        {
            if (Instance && Instance != this as T)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }
    }
}