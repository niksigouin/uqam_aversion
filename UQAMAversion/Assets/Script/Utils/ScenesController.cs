using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesController : MonoBehaviour
{
    [SerializeField] private List<GameObject> activatedSceneObjects;
    
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneChangeController.Instance != null);
        Subscribe();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        SceneChangeController.Instance.FadeOutComplete += StartScene;
    }
    
    private void Unsubscribe()
    {
        SceneChangeController.Instance.FadeOutComplete -= StartScene;
    }
    
    private void StartScene()
    {
        foreach (var activatedSceneObject in activatedSceneObjects)
        {
            activatedSceneObject.SetActive(true);
        }
    }

    public void EndScene()
    {
        SceneChangeController.Instance.EndOfScene?.Invoke();
    }
}
