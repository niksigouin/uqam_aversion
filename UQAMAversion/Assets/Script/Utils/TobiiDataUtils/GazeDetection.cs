using System;
using System.Collections;
using Tobii.G2OM;
using UnityEngine;

namespace Script.Utils
{
    public class GazeDetection : MonoBehaviour, IGazeFocusable
    {
        [SerializeField] private global::PointOfInterest InterestType;
        private float timer;
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => TobiiDataWriterController.Instance != null);
        }

        public void GazeFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                timer = Time.time;
            }
            else
            {
                float lookTime = Time.time - timer;
                if(lookTime < 1f) return;
                TobiiDataWriterController.Instance.POILook?.Invoke(InterestType, lookTime);
            }
        }
    }
}