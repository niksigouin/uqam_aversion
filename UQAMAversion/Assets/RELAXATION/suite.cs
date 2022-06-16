using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suite : MonoBehaviour
{

    public AudioSource audio;
    // Start is called before the first frame update

    float timer = 0;

    bool timerON = true;
    bool soundOn = false;

    public GameObject manager;

    public float timeToPlay = 7f;
     void Update()
    {
        if(timerON == true && timer < timeToPlay){
            timer += Time.deltaTime;
        }

        if(timer  > (timeToPlay - 2) && soundOn == false){
            soundOn = true;
            audio.GetComponent<AudioSource>().Play();
        }

        if(timer  > timeToPlay && timerON == true){
            timerON = false;
            this.GetComponent<relaxation>().respiration = true;
        }
        
        if(timerON == false && !audio.GetComponent<AudioSource>().isPlaying){
                manager.GetComponent<nextScene>().next();
        }
    }

}
