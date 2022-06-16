using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class BreathingObjectController : MonoBehaviour
{
    [Header("Instantiated Object Params")] [SerializeField]
    private GameObject instantiatePrefab;

    [Header("Center Sphere")] 
    [SerializeField] private GameObject centerSphereGameObject;
    [SerializeField] private float centerSphereScale;
    [SerializeField] private LeanTweenType tweenType;
    
    [Header("Breathing params")]
    [SerializeField] private Transform parentContainer;
    [SerializeField] private int amount = 6;
    [SerializeField] private float radius = 2f;
    [FormerlySerializedAs("scale")] [SerializeField] private float spheresAroundScale = 1f;
    [SerializeField] private float spheresAroundTimeToScale = 0.2f;


    [Header("Particle Systems")] 
    [SerializeField] private Transform particleSystemParent;
    [SerializeField] private GameObject particleSystemInhale;
    [SerializeField] private GameObject particleSystemExhale;
    
    [SerializeField] private List<SmallSphere> instantiatedObjects;
    [SerializeField] private GameObject targetSmallSphere;

    private int _centerTween;
    

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SoundCueController._instance != null);
        instantiatedObjects = new List<SmallSphere>(amount);
        InstantiateAroundPoint();
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        SoundCueController._instance.CueTrigger += OnAudioCue;
    }

    private void UnSubscribe()
    {
        SoundCueController._instance.CueTrigger += OnAudioCue;
    }

    public void StartAnimation()
    {
        SoundCueController._instance.PlayAnimation();
    }

    [ContextMenu("Instantiate All")]
    private void InstantiateAroundPoint()
    {
        // int index = (int) Mathf.Floor(val);
        for (int i = 0; i < amount; i++)
        {
            float angle = i * Mathf.PI * 2 / amount;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            var instantiated = Instantiate(instantiatePrefab, pos, rot, parentContainer);
            instantiated.SetActive(false);
            instantiated.transform.localScale = Vector3.zero;
            // instantiated.transform.localScale = new Vector3(spheresAroundScale, spheresAroundScale, spheresAroundScale);
            instantiatedObjects.Add(new SmallSphere(instantiated));
        }
    }

    private void ShowAroundPoint(float val, float ratio)
    {
        int index = (int) Mathf.Floor(val);
        SmallSphere target = instantiatedObjects[index];
        if (target.gameObject.activeSelf == false)
        {
            instantiatedObjects[index].gameObject.SetActive(true);
            if (target.hasTweened) return;
            LeanTween.scale(target.gameObject, new Vector3(spheresAroundScale, spheresAroundScale, spheresAroundScale), spheresAroundTimeToScale);
            target.hasTweened = true;

            // LeanTween.cancel(instantiatedObjects[index]);
        }
        
        // Debug.LogWarning($"Showing at {index}");
    }

    [ContextMenu("Destroy All Instantiated")]
    private void HideAroundPoint(float val, float ratio)
    {
        int index = (int) Mathf.Floor(val);
        SmallSphere target = instantiatedObjects[index];
        if (target.hasTweened) return;
        LeanTween.scale(target.gameObject, Vector3.zero, spheresAroundTimeToScale).setOnComplete(() => target.gameObject.SetActive(false));
        target.hasTweened = true;
        // Debug.LogWarning($"Hiding at {index}");
    }

    private void OnAudioCue(CueType cueType, float cueTime)
    {
        SetSmallSphereTweenBool(false);
        // Debug.Log($"CueType: {cueType} | CueTime: {cueTime}");
        switch (cueType)
        {
            case CueType.Inhale:
                LeanTween.cancel(centerSphereGameObject);
                TweenIn(cueTime);
                break;
            case CueType.Exhale:
                LeanTween.cancel(centerSphereGameObject);
                TweenOut(cueTime);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cueType), cueType, null);
        }
    }

    private void SetSmallSphereTweenBool(bool b)
    {
        foreach (var smallSphere in instantiatedObjects)
        {
            smallSphere.hasTweened = b;
        }
    }

    private void InstantiateParticleSystem(GameObject prefab, float targetTime)
    {
        var gObject = Instantiate(prefab, particleSystemParent);
        var ps = gObject.GetComponent<ParticleSystem>();
        ps.Stop();
        var main = ps.main;
        main.duration = targetTime;
        main.startLifetime = targetTime;
        ps.Play();
    }

    [FormerlySerializedAs("InstantiatedInPS")] [SerializeField] private bool instantiatedInPS = false;
    [FormerlySerializedAs("InstantiatedExPS")] [SerializeField] private bool instantiatedExPS = false;
    
    [ContextMenu("Tween IN")]
    private void TweenIn(float time)
    {
        instantiatedExPS = false;
        float toScale = centerSphereGameObject.transform.localScale.x + centerSphereScale;
        LeanTween.value(gameObject, ShowAroundPoint, 0, amount-1, time).setEase(LeanTweenType.linear);
        LeanTween.scale(centerSphereGameObject, new Vector3(toScale,toScale,toScale), time).setEase(tweenType);
        if (!instantiatedInPS)
        {
            InstantiateParticleSystem(particleSystemInhale, time);
            instantiatedInPS = true;
        }
    }

    [ContextMenu("Tween OUT")]
    private void TweenOut(float time)
    {
        instantiatedInPS = false;
        float toScale = centerSphereGameObject.transform.localScale.x - centerSphereScale;
        LeanTween.value(gameObject, HideAroundPoint, 0, amount-1, time).setEase(LeanTweenType.linear);
        LeanTween.scale(centerSphereGameObject, new Vector3(toScale,toScale,toScale), time).setEase(tweenType);
        if (!instantiatedExPS)
        {
            InstantiateParticleSystem(particleSystemExhale, time);
            instantiatedExPS = true;
        }
    }
}

[Serializable]
class SmallSphere
{
    [SerializeField] public GameObject gameObject;
    [SerializeField] public bool hasTweened;

    public SmallSphere(GameObject targetGameObject)
    {
        gameObject = targetGameObject;
    }
}