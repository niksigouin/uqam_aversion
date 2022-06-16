using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeLookRecord : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject manager;
    void Start()
    {
       manager = GameObject.Find("manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateRecord(){
        manager.GetComponent<nextScene>().goRecord = true;
    }

}
