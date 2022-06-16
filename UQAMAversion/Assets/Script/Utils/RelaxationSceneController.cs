using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxationSceneController : MonoBehaviour
{
    [SerializeField] private GameObject relaxationGameObject;
    // Start is called before the first frame update

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
        SceneChangeController.Instance.FadeOutComplete += StartRelaxation;
        SceneChangeController.Instance.EndOfScene += FinishRelaxationScene;
    }
    
    private void Unsubscribe()
    {
        SceneChangeController.Instance.FadeOutComplete -= StartRelaxation;
        SceneChangeController.Instance.EndOfScene -= FinishRelaxationScene;
    }

    private void StartRelaxation()
    {
        // Debug.Log("STARTING RELAXATION!");
        if (!relaxationGameObject.activeSelf)
        {
            relaxationGameObject.SetActive(true);
        }
        relaxationGameObject.GetComponent<BreathingObjectController>().StartAnimation();
    }

    private void FinishRelaxationScene()
    {
        relaxationGameObject.SetActive(false);
    }

    
}
