using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    // TODO: Intergrate button selection in here and don't do it in a retarded way
    private bool isTriggered = false;

    [SerializeField] private bool isSceneChangeTriggered = false;
    [SerializeField] private bool useSceneTimerForTrigger = false;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Light pointLight;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && isSceneChangeTriggered)
        {
            isTriggered = true;
            if (useSceneTimerForTrigger)
            {
                if(SceneChangeController.Instance.SceneIsFinished) SceneChangeController.Instance.EndOfScene?.Invoke();
            }
            else
            {
                SceneChangeController.Instance.EndOfScene?.Invoke();
            }
            
        }
    }

    public void SetSceneChangeFlag(bool state)
    {
        isSceneChangeTriggered = state;
        meshRenderer.enabled = state;
        pointLight.enabled = state;
    }
}
