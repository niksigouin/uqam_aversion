﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BNG
{
    public class SceneLoader : MonoBehaviour
    {
        [Tooltip("The Unity 'LoadSceneMode' method of loading the scene (In most cases should be 'Single'). ")]
        public LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        [Tooltip("If true, the ScreenFader component will fade the screen to black before loading a level.")]
        public bool UseSceenFader = true;

        [Tooltip(
            "Wait this long in seconds before attempting to load the scene. Useful if you need to fade the screen out before attempting to load the level.")]
        public float ScreenFadeTime = 0.5f;

        [SerializeField] private ScreenFader sf;

        private string _loadSceneName = string.Empty;

        public void LoadScene(string SceneName)
        {
            // Debug.Log($"{this}: LoadScene -----");
            if (UseSceenFader)
            {
                _loadSceneName = SceneName;
                // Debug.Log("Starting fade coroutine");
                StartCoroutine(FadeThenLoadScene());
            }
            else
            {
                // Debug.LogError("UseScreenFader no enabled");
            }
        }
        
        public void FadeOutAndWaitForSceneIndex(string SceneName)
        {
            if (UseSceenFader)
            {
                _loadSceneName = SceneName;
                // Debug.Log("Starting fade coroutine");
                StartCoroutine(FadeAndWait());
            }
            // ADDED THIS ELSE CONDITION TO BY PASS THE FADE IN
            else
            {
                _loadSceneName = SceneName;
                StartCoroutine(WaitForIndexAndLoad());
            }
        }

        public void SkipToIndex(string sceneName)
        {
            _loadSceneName = sceneName;
            StartCoroutine(WaitForIndexAndLoad());
        }

        public IEnumerator FadeThenLoadScene()
        {
            // Debug.Log($"{this}: FadeThenLoadScene -----");
            if (UseSceenFader)
            {
                if (sf == null) sf = FindObjectOfType<ScreenFader>();
                // Debug.Log($"{this}: Inside FadeThenLoadScene");
                if (sf != null)
                    sf.DoFadeIn();
            }
            else
            {
                // Debug.Log("UseScreenFader is not true!");
            }
                
            if (ScreenFadeTime > 0) yield return new WaitForSeconds(ScreenFadeTime);


            SceneManager.LoadScene(_loadSceneName, loadSceneMode);
        }

        public IEnumerator FadeAndWait()
        {
            if (UseSceenFader)
            {
                if (sf == null) sf = FindObjectOfType<ScreenFader>();
                // Debug.Log($"{this}: Inside FadeThenLoadScene");
                if (sf != null)
                    sf.DoFadeIn();
            }
            else
            {
                // Debug.Log("UseScreenFader is not true!");
            }
                
            if (ScreenFadeTime > 0) yield return new WaitForSeconds(ScreenFadeTime);
            yield return new WaitUntil(() => SceneChangeController.Instance.SceneSelectionComplete);
            SceneManager.LoadScene(_loadSceneName, loadSceneMode);
            SceneChangeController.Instance.SceneSelectionComplete = false;
        }

        public IEnumerator WaitForIndexAndLoad()
        {
            yield return new WaitUntil(() => SceneChangeController.Instance.SceneSelectionComplete);
            SceneManager.LoadScene(_loadSceneName, loadSceneMode);
            SceneChangeController.Instance.SceneSelectionComplete = false;
        }
    }
}