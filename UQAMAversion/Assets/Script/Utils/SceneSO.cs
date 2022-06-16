using Unity.Collections;
using UnityEngine;

namespace Script.Utils
{
    [CreateAssetMenu(fileName = "New SceneInfo", menuName = "AVERSION SCENES", order = 0)]
    public class SceneSO : ScriptableObject
    {
        [SerializeField] private string name = "sequence";
        [SerializeField] private string sceneName = "SceneName";
        
        [Header("Scene Settings")]
        [SerializeField] private float bleuScreenWaitTime = 3f;
        [SerializeField] private bool useSceneTimer = false;
        [SerializeField] private bool useSceneTimerAsTrigger = true;
        [SerializeField] private float sceneTimer = 10f;

        [SerializeField] private bool sceneUsesLocomotion = true;
        [SerializeField] private bool sceneUsesVisibleHands = true;
        
        [Header("Order Setup")]
        [SerializeField] private bool pickMeAtRandom = true;

        [SerializeField] private bool hasBeenPicked = false;

        [SerializeField] private bool isLastScene = false;

        public string SceneName => sceneName;

        public bool UseSceneTimer => useSceneTimer;

        public float SceneTimer => sceneTimer+bleuScreenWaitTime;

        public float BleuScreenWaitTime => bleuScreenWaitTime;

        public bool UseSceneTimerAsTrigger => useSceneTimerAsTrigger;
        public bool SceneUsesLocomotion => sceneUsesLocomotion;

        public bool SceneUsesVisibleHands => sceneUsesVisibleHands;

        public bool PickMeAtRandom => pickMeAtRandom;
        public bool HasBeenPicked => hasBeenPicked;

        public bool UseForRandom => pickMeAtRandom;

        public bool IsLastScene => isLastScene;
        // include scene spawn location
        // include use timer to end scene

    }
}