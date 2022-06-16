using System;
using System.Collections;
using UnityEngine;

namespace Script.Utils
{
    public class GenreSelectController : MonoBehaviour
    {
        [SerializeField] private ZoneTrigger _zoneTrigger;
        public void SetPrefGenre(bool target)
        {
            SceneChangeController.Instance.SetPrefGenre(target ? Genre.Orange : Genre.Purple);
            _zoneTrigger.SetSceneChangeFlag(true);
        }
    }
}