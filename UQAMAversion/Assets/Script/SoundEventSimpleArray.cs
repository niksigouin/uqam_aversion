using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundEventSimpleArray : MonoBehaviour {

    public AudioClip[] sourcesAudio;

    public GameObject audioSourceParent;
    private AudioSource audioPlayer;
    private bool turnUp;
    private bool exit;

    private int idSource;
    private int sourceRand;

    private bool fadeOn;

    void Awake(){
        audioPlayer = audioSourceParent.GetComponent<AudioSource>();
        fadeOn = false;
        turnUp = false;
        exit = false;
    }

    void Update(){
        if(exit == true && turnUp == true){
            turnUp = false;
            StopCoroutine("FaderAudio");
            exit = false;
        }

    }
    void jouerSonSimple(int IDSource){
    
    
            sourceRand = IDSource;

        
       

        
        

        if(audioPlayer.isPlaying){
            if(fadeOn == true){
                StopCoroutine("FaderAudio");
                fadeOn = false;
            }
            StartCoroutine("FaderAudio");
        }

        else{
            audioPlayer.clip = sourcesAudio[sourceRand];
            audioPlayer.Play();
        }
        
	}
    
    IEnumerator FaderAudio()
    {
        fadeOn = true;
           while (turnUp == false && exit == false)
            {
                audioPlayer.volume -= 0.01f;
                if(audioPlayer.volume <= 0f){
                   
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
