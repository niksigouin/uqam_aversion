using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCueController : MonoBehaviour
{
    public static SoundCueController _instance;
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _cueBuffer = 0.01f;
    [SerializeField] private List<SoundCue> _soundCues;

    public Action AudioStart;
    public Action<CueType, float> CueTrigger;

    void Start()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;

        _instance.AudioStart += PlayAnimation;
    }

    [ContextMenu("Play Sound")]
    public void PlayAnimation()
    {
        _audioSource.Play();
    }

    public void StopAnimation()
    {
        _audioSource.Stop();
    }

    

    private void GetCues()
    {
        foreach (var cue in _soundCues)
        {
            if (_audioSource.time >= cue.CueIndex.x - _cueBuffer && _audioSource.time <= cue.CueIndex.x + _cueBuffer)
            {
                // Debug.Log($"Playhead is at {cue.Name}");
                _instance.CueTrigger?.Invoke(cue.CueType, cue.CueTime);
            }
        }
    }

    private void FixedUpdate()
    {
        if(_audioSource.isPlaying) GetCues();
    }
}

[Serializable]
public class SoundCue
{
    [SerializeField] private string name;
    [SerializeField] private Vector2 cueIndex;
    [SerializeField] private CueType cueType;
    [SerializeField] private bool hasTriggered = false;

    public string Name => name;
    public Vector2 CueIndex => cueIndex;

    public CueType CueType => cueType;

    public float CueTime => CueIndex.y - CueIndex.x;

    public bool HasTriggered { get => hasTriggered; set => hasTriggered = value; }
    
    
}

[Serializable]
public enum CueType
{
    Inhale = 0,
    Exhale = 1
}