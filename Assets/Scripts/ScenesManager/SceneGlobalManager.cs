using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScenesManager
{
    public class SceneGlobalManager : MonoBehaviour
    {
        public static SceneGlobalManager Instance { get; private set; }

        private List<SceneConfiguration> _loadedScenes;
        private List<AsyncOperation> _loadingProcesses;

        public static Action<float> OnProgress;
        public static Action OnStartProgress;
        public static Action OnFinishProgress;

        private IEnumerator _AsyncProcesses;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this.gameObject);

            Instance = this;

            _AsyncProcesses = AsyncProcess();

            _loadedScenes = new();
            _loadingProcesses = new();
        }

        public void LoadScene(SceneConfiguration SceneToLoad)
        {
            _loadedScenes.Add(SceneToLoad);
            _loadingProcesses.Add(SceneToLoad.LoadScene());

            StartCoroutine(_AsyncProcesses);
        }

        public void LoadScene(SceneConfiguration SceneToLoad, SceneConfiguration CurrentScene = null)
        {
            _loadedScenes.Add(SceneToLoad);
            Debug.Log(SceneToLoad.LoadScene().progress);
            _loadingProcesses.Add(SceneToLoad.LoadScene());

            if (CurrentScene != null)
            {
                _loadedScenes.Remove(CurrentScene);
                _loadingProcesses.Add(CurrentScene.UnloadScene());
            }

            StartCoroutine(_AsyncProcesses);
        }

        public void UnloadScene(SceneConfiguration SceneToUnload)
        {
            _loadedScenes.Remove(SceneToUnload);
            _loadingProcesses.Add(SceneToUnload.UnloadScene());

            StartCoroutine(_AsyncProcesses);
        }

        private IEnumerator AsyncProcess()
        {
            OnStartProgress?.Invoke();

            yield return new WaitForSeconds(0.5f);

            float totalProgress = 0;
            for (int i = 0; i < _loadingProcesses.Count; i++)
            {
                while (!_loadingProcesses[i].isDone)
                {
                    totalProgress += _loadingProcesses[i].progress;

                    OnProgress?.Invoke(totalProgress / _loadingProcesses.Count);

                    yield return null;
                }
            }

            yield return new WaitForSeconds(0.5f);

            OnFinishProgress?.Invoke();

            _loadingProcesses = new();

            _AsyncProcesses = AsyncProcess();
        }
    }
}