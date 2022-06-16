using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.XR;

public class LightSwitchController : MonoBehaviour
{
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private bool isSceneChangeTriggered = false;
    [SerializeField] private bool useHaptic = false;
    [SerializeField] private AudioSource aS;

    [SerializeField] private List<Light> lights;
    
    
    public bool IsSceneChangeTriggered => isSceneChangeTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (useHaptic && other.gameObject.TryGetComponent(typeof(Grabber), out Component handController))
        {
            InputBridge.Instance.VibrateController(0.2f, 0.25f, 0.2f, handController.GetComponent<Grabber>().HandSide);
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).SendHapticImpulse(0, 0.25f, 0.2f);
        }

        if (!isSceneChangeTriggered)
        {
            isSceneChangeTriggered = true;
        }

        if (!isTriggered)
        {
            ToggleLights();
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggered = false;
    }

    private void PlaySound(bool state)
    {
        
        if (state)
        {
            aS.pitch = 1f;
            aS.Play();
        }
        else
        {
            aS.pitch = 0.8f;
            aS.Play();
        }
    }

    private void ToggleLights()
    {
        foreach (var tLight in lights)
        {
            if (tLight.intensity < 1)
            {
                tLight.intensity = 1;
                PlaySound(true);
            }
            else
            {
                tLight.intensity = 0;
                PlaySound(false);
            }
        }
    }
}
