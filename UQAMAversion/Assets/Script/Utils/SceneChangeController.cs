using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using BNG;
using Script.Utils;
using Tobii.XR.GazeModifier;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private SceneSO bufferScene;

    [Header("Script References")] 
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private VRInputController vrInputController;

    [Header("XR Rig")] 
    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject playerController;
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private GameObject playerCamera;

    [Header("Scene information")] 
    [SerializeField] private bool sceneIsFinished = false;
    public bool canGoToNextScene = false;

    [SerializeField] private List<SceneSO> RandomSceneList;
    [SerializeField] private GameObject GoToIndicator;

    [Header("Persistent Information")] 
    private Genre _prefGenre = Genre.None;

    public GameObject PlayerCamera => playerCamera;

    public bool SceneIsFinished
    {
        get { return sceneIsFinished; }
    }
    
    public bool SceneSelectionComplete { get; set; } = false;

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
        // vrInputController.ToggleHands(sceneList[currentSceneIndex].SceneUsesVisibleHands);
        // ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
    }

    private void OnDestroy()
    {
        Debug.Log($"{this}: Getting Destroyed!");
        Unsubscribe();
    }

    private void Update()
    {
        if (!SceneSelectionComplete)
        {
            if (InputBridge.Instance.AButtonDown || Input.GetKey(KeyCode.RightArrow))
            {
                LoadNextScene();
                SceneSelectionComplete = true;
            }
            else if (InputBridge.Instance.XButtonDown || Input.GetKey(KeyCode.LeftArrow))
            {
                LoadPreviousScene();
                SceneSelectionComplete = true;
            }
            
            // TODO: DON'T DO THIS, DO SOMETHING ELSE THAT'S LESS COMPUTATIONALLY DEMANDING
            foreach (char c in Input.inputString)
            {
                if (char.IsDigit(c))
                {
                    int index;
                    if (int.TryParse(c.ToString(), out index))
                    {
                        Debug.Log($"Number was pressed: {index} with type {index.GetType()}");
                        if (index >= 0 || index <= sceneList.Count)
                        {
                            LoadSceneAtIndex(index);
                            SceneSelectionComplete = true;
                        }
                        else
                        {
                            Debug.LogWarning($"{this}: They sceneIndex selected is not valid, please chose an index within the range of the loaded scene selection");
                        }
                    }
                }
            }
        }
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
        sceneList = preSceneList;
        
        // List<SceneSO> tempSceneList = new List<SceneSO>();
        // debugSceneList = preSceneList;
        // Debug.Log("[SCENE SHUFFLE] Copied pre-scene list to debug list");
        //
        // //for each index in shuffledSceneIndexes, add the corresponding indexes from preScenelist to the tempSceneList
        // foreach (var index in shuffledSceneIndexes)
        // {
        //     tempSceneList.Add(preSceneList[index]);
        // }
        //
        // //Shuffle the tempSceneList 
        // Shuffle(tempSceneList);
        //
        // //For each index in shuffledSceneIndexes, replace the corresponding indexes from tempSceneList in the preSceneList
        // for (int i = 0; i < shuffledSceneIndexes.Count; i++)
        // {
        //     preSceneList[shuffledSceneIndexes[i]] = tempSceneList[i];
        //     Debug.Log($"[SCENE SHUFFLE] Now replacing index {preSceneList[shuffledSceneIndexes[i]].SceneName} with {tempSceneList[i].SceneName}");
        // }
        //
        // //Add the preSceneList to the sceneList
        //
        // foreach (int index in shuffledSceneIndexes)
        // {
        //     tempSceneList.Add(sceneList[index]);
        // }
        //
        // Shuffle(RandomSceneList);
        // //TODO: MAKE THIS LESS DUMB.... AND MORE USABLE... im not proud
        // //TODO: ADD AN OTHER EXPOSED FIELD THAT HOLDS THE INDEXES OF THE DESIRED SCENES TO SHUFFLE (IE: {2, 4})
        // //TODO: IF THE CURRENT TARGET INDEX IN PRESCENELIST IS NOT PRESENT IN REFERENCE LIST
        // for (int i = 0; i < preSceneList.Count; i++)
        // {
        //     if (i == 0 || i == 1)
        //     {
        //         sceneList.Add(preSceneList[i]);
        //     }
        //     else if (i == 2)
        //     {
        //         sceneList.AddRange(RandomSceneList);
        //     } else if (i == 3)
        //     {
        //         Debug.Log("skippyinh scene place");
        //     }
        //     else if (i == 4)
        //     {
        // sceneList.Add(preSceneList[4]);
        //     }
        //     // Debug.Log(i);
        // }


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
        // LoadNextScene();
        screenFader.DoFadeIn();
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
    
    [ContextMenu("Unload Current Scene")]
    private void UnloadCurrentScene()
    {
        SceneManager.UnloadSceneAsync(currentSceneIndex);
    }

    [ContextMenu("Load Scene At Index")]
    private void LoadSceneAtIndex(int sceneIndex)
    {
        currentSceneIndex = sceneIndex;
        sceneLoader.SkipToIndex(sceneList[currentSceneIndex].SceneName);
        sceneIsFinished = false;
        canGoToNextScene = false;
    }
    
    [ContextMenu("Load Next Scene")]
    private void LoadNextScene()
    {
        // Apply scene params (wait times and others)
        
        // ApplyBleuScreenTimer(sceneList[currentSceneIndex]);
        currentSceneIndex++;
        Debug.Log(
            $"{this}: Called Load Next Scene! | Loading [{sceneList[currentSceneIndex].SceneName}] at index [{currentSceneIndex}]");
        if (currentSceneIndex <= sceneList.Count - 1)
        {
            // sceneLoader.LoadScene(sceneList[currentSceneIndex].SceneName);
            sceneLoader.FadeOutAndWaitForSceneIndex(sceneList[currentSceneIndex].SceneName);
        }
        // currentSceneIndex++;
        sceneIsFinished = false;
        canGoToNextScene = false;
    }

    [ContextMenu("Load Previous Scene")]
    private void LoadPreviousScene()
    {
        // currentSceneIndex--;
        if (currentSceneIndex >= 0)
        {
            sceneLoader.FadeOutAndWaitForSceneIndex(sceneList[currentSceneIndex].SceneName);
        }
        sceneIsFinished = false;
        canGoToNextScene = false;
        // sceneLoader.LoadScene(sceneList[currentSceneIndex].SceneName);
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
            canGoToNextScene = false;
            StartCoroutine(StartSceneTimer(sceneList[currentSceneIndex].SceneTimer, sceneList[currentSceneIndex].UseSceneTimerAsTrigger));
        }
    }

    private IEnumerator StartSceneTimer(float time, bool isTrigger)
    {
        Debug.Log($"{this}: Started scene timer with {time} and {isTrigger}");
        canGoToNextScene = false;
        yield return new WaitForSeconds(time);
        Debug.Log("Scene timer was completed!");
        canGoToNextScene = true;
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
            canGoToNextScene = true;
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

