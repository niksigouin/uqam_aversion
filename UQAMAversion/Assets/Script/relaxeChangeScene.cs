using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class relaxeChangeScene : MonoBehaviour
{
    public AudioSource son;

    public GameObject manager;

    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(active == true){
            if(!son.isPlaying){
                active = false;
                manager.GetComponent<nextScene>().next();
            }
        }
        
    }
}
