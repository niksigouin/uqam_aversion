using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Script.Utils
{
    public class PlayerGenreController : MonoBehaviour
    {
        [Header("Dev Settings")] [SerializeField]
        private bool disableOnStart = true;
        [Header("Characters GameObjects")]
        [SerializeField] private GameObject male;
        [SerializeField] private GameObject female;
        private IEnumerator Start()
        {
            if(disableOnStart) DisbaleCharactersOnStart();
            yield return new WaitUntil(() => SceneChangeController.Instance != null);
            ApplyGenrePref();
            Subscribe();
        }
        
        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            SceneChangeController.Instance.FadeOutComplete += StartAnimations;
        }

        private void StartAnimations()
        {
            switch (SceneChangeController.Instance.PrefGenre)
            {
                case Genre.Orange:
                    male.GetComponent<Animator>().SetBool("ready", true);
                    break;
                case Genre.Purple:
                    female.GetComponent<Animator>().SetBool("ready", true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Unsubscribe()
        {
            SceneChangeController.Instance.FadeOutComplete -= StartAnimations;
        }

        private void ApplyGenrePref()
        {
            switch (SceneChangeController.Instance.PrefGenre)
            {
                case Genre.Orange:
                    Destroy(female);
                    male.SetActive(true);
                    break;
                case Genre.Purple:
                    Destroy(male);
                    female.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisbaleCharactersOnStart()
        {
            male.SetActive(false);
            female.SetActive(false);
        }
    }
}