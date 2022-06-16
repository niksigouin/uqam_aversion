using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using Tobii.G2OM;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.XR;

namespace Script.Utils
{
    public class PointOfInterest : MonoBehaviour, IGazeFocusable
    {
        [Header("Settings")]
        [SerializeField] private bool useHaptic = false;
        [SerializeField] private bool logGaze = false;
        [SerializeField] private bool logTouch = false;
        [SerializeField] private bool logGrab = false;
        [SerializeField] private global::PointOfInterest InterestType;
        
        private float gazeTimer;
        private float touchTimer;
        private float grabTimer;
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => TobiiDataWriterController.Instance != null);
        }
        
        public void GazeFocusChanged(bool hasFocus)
        {
            if (!logGaze) return;
            if (hasFocus)
            {
                gazeTimer = Time.time;
            }
            else
            {
                float lookTime = Time.time - gazeTimer;
                if(lookTime < 1f) return;
                TobiiDataWriterController.Instance.POILook?.Invoke(InterestType, lookTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!logTouch) return;
            if (other.gameObject.TryGetComponent(typeof(Grabber), out Component handController))
            {
                touchTimer = Time.time;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!useHaptic) return;
            if (other.gameObject.TryGetComponent(typeof(Grabber), out Component handController))
            {
                InputBridge.Instance.VibrateController(0.2f, 0.25f, 0.2f, handController.GetComponent<Grabber>().HandSide);
                if(handController.GetComponent<Grabber>().HandSide == ControllerHand.Left) PXR_Input.SetControllerVibration(0.5f, 2, PXR_Input.Controller.LeftController);
                else if (handController.GetComponent<Grabber>().HandSide == ControllerHand.Right) PXR_Input.SetControllerVibration(0.5f, 2, PXR_Input.Controller.RightController);
                
            }
        }

        public void OnGrabEvent()
        {
            if(!logGrab) return;
            grabTimer = Time.time;
        }

        public void OnReleaseGrab()
        {
            if(!logGrab) return;
            float grabTime = Time.time - grabTimer;
            // if(grabTime < 1f) return;
            TobiiDataWriterController.Instance.POIGrab?.Invoke(InterestType, grabTime);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!logTouch) return;
            if (other.gameObject.TryGetComponent(typeof(Grabber), out Component handController))
            {
                float touchTime = Time.time - touchTimer;
                // if(touchTime < 0.75f) return;
                TobiiDataWriterController.Instance.EroticZoneTouchEvent?.Invoke(InterestType, touchTime);
            }
        }
    }
}