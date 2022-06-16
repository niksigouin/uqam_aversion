using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.XR;

public class SceneChangeTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    private bool isSceneChangeTriggered = false;
    [SerializeField] private bool useHaptic = false;

    private void Subscribe()
    {
        SceneChangeController.Instance.SceneTimerDone += SceneTimerDone;
    }

    private void Unsubscribe()
    {
        SceneChangeController.Instance.SceneTimerDone -= SceneTimerDone;
    }

    private void SceneTimerDone()
    {
        if (isSceneChangeTriggered)
        {
            // SceneChangeController.Instance.EndOfScene?.Invoke();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && SceneChangeController.Instance.SceneIsFinished)
        {
            isTriggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (useHaptic && other.gameObject.TryGetComponent(typeof(Grabber), out Component handController))
        {
            InputBridge.Instance.VibrateController(0.2f, 0.25f, 0.2f, handController.GetComponent<Grabber>().HandSide);
            if(handController.GetComponent<Grabber>().HandSide == ControllerHand.Left) InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0,0.5f,2f);
            else if (handController.GetComponent<Grabber>().HandSide == ControllerHand.Right) InputDevices.GetDeviceAtXRNode(XRNode.RightHand).SendHapticImpulse(0,0.5f,2f);
        }
    }
}
