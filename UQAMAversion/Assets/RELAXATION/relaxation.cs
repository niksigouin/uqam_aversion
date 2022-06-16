using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class relaxation : MonoBehaviour
{

    public GameObject objToScale;
    public Vector3 scaleMin;
    public Vector3 scaleMax;

    public Light lightToGrad;
    public float powerMin = 0;
    public float powerMax;
    public float rangeMin;
    public float rangeMax;
    public float currentTime;
     public float currentTimeSphere;

    public float timerMax;

    public float incrementTimer;

    public bool inspire = true;
    public bool respiration = true;

    public ParticleSystem partUP;

    public ParticleSystem partDOWN;

    public AudioSource audio;

    public GameObject[] littleCircles;

    

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Started Relaxation");
        scaleMin = objToScale.transform.localScale;
        rangeMin = lightToGrad.range;


        partDOWN.Stop();
        partUP.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(respiration == true && audio.isPlaying){

            

            if (inspire == true){

                    if (!partDOWN.isPlaying || partUP.isPlaying)
                    {
                        partDOWN.Play();
                        partUP.Stop();
                    }
         
                currentTime += Time.deltaTime;
                currentTimeSphere += Time.deltaTime;

                if(currentTime >= timerMax){
                    currentTime = 0;
                    currentTimeSphere = timerMax;
                    inspire = false;
                    partUP.Stop();
                    partDOWN.Stop();
                }

                if(littleCircles[(int)Mathf.Floor(currentTime*12/timerMax)] != null && (int)Mathf.Floor(currentTime*12/timerMax)<littleCircles.Length){
                    littleCircles[(int)Mathf.Floor(currentTime*12/timerMax)].SetActive(true);
                }
            }
            else{

                
                currentTime += Time.deltaTime;
                currentTimeSphere -= Time.deltaTime;


                    if (!partUP.isPlaying || partDOWN.isPlaying)
                    {
                        partUP.Play();
                        partDOWN.Stop();
                    }

                    if(currentTime >= timerMax){
                    currentTime = 0;
                    currentTimeSphere = 0;
                    inspire = true;
                    partUP.Stop();
                    partDOWN.Stop();
                }

                    if(littleCircles[(int)Mathf.Floor(currentTime*12/timerMax)] != null && (int)Mathf.Floor(currentTime*12/timerMax)< littleCircles.Length){
                        littleCircles[(int)Mathf.Floor(currentTime*12/timerMax)].SetActive(false);
                    }
                
            }

        
            lightToGrad.intensity = currentTimeSphere * powerMax / timerMax;
            lightToGrad.range = (currentTimeSphere * rangeMax / timerMax)+ rangeMin;
            var scale = currentTimeSphere * scaleMax.x / timerMax;
            objToScale.transform.localScale = new Vector3(scale,scale,scale);

    
        }
        
    }
}
