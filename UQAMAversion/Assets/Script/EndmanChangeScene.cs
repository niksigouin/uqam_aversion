using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndmanChangeScene : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject manager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextScene(){
        manager.GetComponent<nextScene>().next();
    }
}
