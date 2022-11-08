using System;
using System.Collections;
using System.Xml.Schema;
using BNG;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Utils
{
    public class GenreSelectController : MonoBehaviour
    {
        [SerializeField] private ZoneTrigger _zoneTrigger;

        private void Update()
        {
            if (InputBridge.Instance.YButtonDown || Input.GetKey("up"))
            {
                SetPrefGenre(true);
            } else if (InputBridge.Instance.XButtonDown || Input.GetKey("down"))
            {
                SetPrefGenre(false);
            }
        }

        public void SetPrefGenre(bool target)
        {
            SceneChangeController.Instance.SetPrefGenre(target ? Genre.Orange : Genre.Purple);
            SceneChangeController.Instance.EndOfScene?.Invoke();
            // _zoneTrigger.SetSceneChangeFlag(true);
        }
    }
}