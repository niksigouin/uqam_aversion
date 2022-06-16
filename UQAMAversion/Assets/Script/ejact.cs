using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ejact : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem sperm;
    public ParticleSystem sperm2;

    public bool timer = false;

    public float timeF;

    private Vector3 scale;
    public GameObject stain;
    void Start()
    {
        scale = stain.transform.localScale;

        stain.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer == true){
            timeF += Time.deltaTime;
            if(timeF>1f){
                if(stain){
                    stain.SetActive(true);
                    stain.transform.localScale = Vector3.Lerp (stain.transform.localScale, scale, Time.deltaTime);

                    if(stain.transform.localScale == scale){
                        timer = false;
                    }
                }
                
                
            }
        }
    }

    void shoot(){
        sperm.Play();
        sperm2.Play();
       timer = true;

    }

}
