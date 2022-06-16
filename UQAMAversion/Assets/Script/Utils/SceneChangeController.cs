using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BNG;
using Script.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneChangeController : MonoBehaviour
{
    public Action FadeInComplete;
    public Action FadeOutComplete;
    public Action EndOfScene;
    public Action SceneTimerDone;

    private static SceneChangeController _instance;
    public static SceneChangeController Instance { get { return _instance; } }
    private int currentSceneIndex = 0;

    public Transform ikTarget = null;
    [Header("Scenes")]
    [SerializeField] List<SceneSO> sceneList = new List<SceneSO>(5);
    [SerializeField] List<SceneSO> preSceneList = new List<SceneSO>(5);

    [Header("Script References")] 
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private VRInputController vrInputController;

    [Header("XR Rig")] 
    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject playerController;
    [SerializeField] private ScreenFader screenFader;

    [Header("Scene information")] 
    [SerializeField] private bool sceneIsFinished = false;

    [SerializeField] private List<SceneSO> RandomSceneList;
    [SerializeField] private GameObject GoToIndicator;

    [Header("Persistent Information")] 
    private Genre _prefGenre = Genre.None;
    
    

    public bool SceneIsFinished
    {
        get { return sceneIsFinished; }
    }

    public Genre PrefGenre => _prefGenre;

    private Transform spawnLocation;
    private void Awake()
    {
        // Singleton pattern setting
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
        
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(xrRig);
        
        Subscribe();

        BuildSceneList();
    }

    private void Start()
    {
        vrInputController.ToggleHands(sceneList[currentSceneIndex].SceneUsesVisibleHands);
        // ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    public void ToggleGoToIndicator(bool state)
    {
        GoToIndicator.SetActive(state);
    }

    private void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    private void BuildSceneList()
    {
        Shuffle(RandomSceneList);
        //TODO: MAKE THIS LESS DUMB.... AND MORE USABLE... im not proud
        for (int i = 0; i < preSceneList.Count; i++)
        {
            if (i == 0 || i == 1)
            {
                sceneList.Add(preSceneList[i]);
            }
            else if (i == 2)
            {
                sceneList.AddRange(RandomSceneList);
            } else if (i == 3)
            {
                Debug.Log("skippyinh scene place");
            }
            else if (i == 4)
            {
                sceneList.Add(preSceneList[4]);
            }
            // Debug.Log(i);
        }
    }

    //On change scenes, disable inputs and stuff
    
    private void Subscribe()
    {
        Instance.EndOfScene += OnEndOfScene;
        Instance.FadeInComplete += OnFadeInComplete;
        Instance.FadeOutComplete += OnFadeOutComplete;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void Unsubscribe()
    {
        Instance.EndOfScene -= OnEndOfScene;
        Instance.FadeInComplete -= OnFadeInComplete;
        Instance.FadeOutComplete -= OnFadeOutComplete;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // TeleportPlayerToSpawn(spawnLocation);
        
        vrInputController.ToggleHands(sceneList[currentSceneIndex].SceneUsesVisibleHands);
        Debug.LogWarning($"Current scene index {currentSceneIndex} with hands {sceneList[currentSceneIndex].SceneUsesVisibleHands}");
    }
    
    private void OnEndOfScene()
    {
        ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
        LoadNextScene();
    }

    private void OnFadeInComplete()
    {
        //if(currentSceneIndex != 0) 
            // vrInputController.ToggleHands(false);
            
        vrInputController.TogglePlayerGravity(false);
        vrInputController.TogglePlayerMouvement(false);
        
        //Get next scene spawn location and apply to VR Rig Transform
        
    }
    
    private void OnFadeOutComplete()
    {
        ApplySceneTimer();
        // SetTPFade();
        // ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
        StartCoroutine(SetPlayerGravityAndMouvement());
    }

    private IEnumerator SetPlayerGravityAndMouvement()
    {
        yield return new WaitUntil(() => vrInputController != null);
        vrInputController.TogglePlayerGravity(sceneList[currentSceneIndex].SceneUsesLocomotion);
        vrInputController.TogglePlayerMouvement(sceneList[currentSceneIndex].SceneUsesLocomotion);
    }
    
    [ContextMenu("Load Next Scene")]
    private void LoadNextScene()
    {
        // Apply scene params (wait times and others)
        
        // ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
        currentSceneIndex++;
        Debug.Log(
            $"{this}: Called Load Next Scene! | Loading [{sceneList[currentSceneIndex].SceneName}]");
        if (currentSceneIndex <= sceneList.Count - 1)
        {
            sceneLoader.LoadScene(sceneList[currentSceneIndex].SceneName);
        }
        // currentSceneIndex++;
        sceneIsFinished = false;
    }

    public void SetPlayerTransform(Transform targetTransform)
    {
        // playerController.transform.position = Vector3.zero;
        
        // xrRig.transform.position = targetTransform.position;
        // xrRig.transform.rotation = targetTransform.rotation;
        // playerController.transform.rotation = Quaternion.identity;

        spawnLocation = targetTransform;
    }

    public void TeleportPlayerToSpawn(Transform targetTransform)
    {
        playerController.GetComponent<PlayerTeleport>().FadeScreenOnTeleport = false;
        playerController.GetComponent<PlayerTeleport>().TeleportPlayerToTransform(targetTransform);
        playerController.GetComponent<PlayerTeleport>().FadeScreenOnTeleport = true;
    }
    

    private void ApplyBleuScreenTimer(SceneSO targetSO)
    {
        // Grab initial scene info
        Debug.Log($"{this}: Applied scene blue screen delay with delay {targetSO.BleuScreenWaitTime}");
        screenFader.SceneFadeInDelay = targetSO.BleuScreenWaitTime;
        screenFader.FadeColor = Color.blue;
        screenFader.SetFadeColor(Color.blue);
        screenFader.FadeInSpeed = 0.5f;
        screenFader.FadeOutSpeed = 1f;
    }

    private void SetTPFade()
    {
        screenFader.FadeColor = Color.black;
        screenFader.SetFadeColor(Color.black);
        screenFader.FadeInSpeed = 5f;
        screenFader.FadeOutSpeed = 5f;
        Debug.LogWarning("##### SET TP FADE #####");
    }

    private void ApplySceneTimer()
    {
        if (sceneList[currentSceneIndex].UseSceneTimer)
        {
            Debug.Log($"{this}: Applied scene timer with {sceneList[currentSceneIndex].SceneTimer}");
            StartCoroutine(StartSceneTimer(sceneList[currentSceneIndex].SceneTimer, sceneList[currentSceneIndex].UseSceneTimerAsTrigger));
        }
    }

    private IEnumerator StartSceneTimer(float time, bool isTrigger)
    {
        Debug.Log($"{this}: Started scene timer with {time} and {isTrigger}");
        yield return new WaitForSeconds(time);
        Debug.Log("Scene timer was completed!");
        Instance.SceneTimerDone?.Invoke();
        // Debug.Log("THIS IS THE END OF THE SCENE");
        
        //For last scene purposes
        if (sceneList[currentSceneIndex].IsLastScene)
        {
            screenFader.DoFadeIn();
            FindObjectOfType<SoundCueController>().StopAnimation();
        }
        
        if (isTrigger)
        {
            Instance.EndOfScene?.Invoke();
            
            // LoadNextScene();
        }
        else
        {
            sceneIsFinished = true;
        }
    }

    public void SetPrefGenre(Genre target)
    {
        _prefGenre = target;
        Debug.Log($"Set target genre to {target.ToString()}");
        ToggleGoToIndicator(true);
    }
    
}

public enum Genre
{
    Orange = 1,
    Purple = 2,
    None = 0
}

