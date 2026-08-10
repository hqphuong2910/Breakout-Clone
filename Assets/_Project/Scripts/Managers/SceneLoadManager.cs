using System.Collections;
using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Managers
{
    public class SceneLoadManager : Singleton<SceneLoadManager>
    {
        [Header("Dependencies")] [SerializeField]
        private GameConfigSO gameConfig;

        #region ASYCN_LOADING_ROUTINE

        private IEnumerator LoadSceneAsyncRoutine(string sceneName)
        {
            AppLogger.Log(name, $"Starting async load for scene: {sceneName}");
            // UIEvents.OnOpenScreen?.Invoke(ScreenType.LoadingScreen);

            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            if (asyncOperation != null)
            {
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    var progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

                    if (asyncOperation.progress >= 0.9f) asyncOperation.allowSceneActivation = true;

                    yield return null;
                }
            }

            AppLogger.Log(name, $"Successfully loaded scene: {sceneName}.");

            // UIEvents.OnCloseTopScreen?.Invoke();
        }

        #endregion

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            SceneEvents.OnRequestLoadGameplay += LoadGameplayScene;
            SceneEvents.OnRequestLoadMainMenu += LoadMainMenuScene;
            SceneEvents.OnRequestLoadScene += LoadSceneByName;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            SceneEvents.OnRequestLoadGameplay -= LoadGameplayScene;
            SceneEvents.OnRequestLoadMainMenu -= LoadMainMenuScene;
            SceneEvents.OnRequestLoadScene -= LoadSceneByName;
        }

        #endregion

        #region SCENE_HANDLERS

        private void LoadGameplayScene()
        {
            if (!gameConfig) return;
            SceneManager.LoadScene(gameConfig.gameplaySceneName);
        }

        private void LoadMainMenuScene()
        {
            if (!gameConfig) return;
            SceneManager.LoadScene(gameConfig.mainMenuSceneName);
        }

        private void LoadSceneByName(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                AppLogger.LogError(name, "Scene name is null or empty.");
                return;
            }

            StartCoroutine(LoadSceneAsyncRoutine(sceneName));
        }

        #endregion
    }
}