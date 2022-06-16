using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class writeAnimEntry : MonoBehaviour
{

    public GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void writeEntry(string animName){

        manager.GetComponent<lookDetect>().writeAnimStart("StartAnim : " + animName);
    }
}
