using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundEventSingle : MonoBehaviour
{

    public AudioClip[] sourcesAudio;

    public GameObject audioSourceParent;
    private AudioSource audioPlayer;
    private bool turnUp;
    private bool exit;

    private int idSource;
    private int sourceRand;

    private bool fadeOn;

    public bool variation = false;
    void Awake()
    {
        audioPlayer = audioSourceParent.GetComponent<AudioSource>();
        fadeOn = false;
        turnUp = false;
        exit = false;
    }

    void Update()
    {
        if (exit == true && turnUp == true)
        {
            turnUp = false;
            StopCoroutine("FaderAudio");
            exit = false;
        }

    }
    void jouerSonSingle(int IDSource)
    {

    
        sourceRand = IDSource;
        

        if(variation == true){
            sourceRand = Random.Range(0,sourcesAudio.Length);
            audioPlayer.pitch = Random.Range(1,2);
        }
        




        if (audioPlayer.isPlaying)
        {
            if (fadeOn == true)
            {
                StopCoroutine("FaderAudio");
                fadeOn = false;
            }
            StartCoroutine("FaderAudio");
        }

        else
        {
            audioPlayer.clip = sourcesAudio[sourceRand];
            audioPlayer.Play();
        }

    }

    IEnumerator FaderAudio()
    {
        fadeOn = true;
        while (turnUp == false && exit == false)
        {
            audioPlayer.volume -= 0.1f;
            if (audioPlayer.volume <= 0f)
            {

                audioPlayer.clip = sourcesAudio[sourceRand];
                turnUp = true;
            }
            yield return null;
        }

        while (turnUp == true && exit == false)
        {
            audioPlayer.volume += 0.1f;
            if (audioPlayer.volume >= 0.9f)
            {
                audioPlayer.Play();

                exit = true;


            }
            yield return null;
        }

    }
}
