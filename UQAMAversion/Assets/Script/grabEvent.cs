using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    public GameObject holder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void grab(){
        if(obj.GetComponent<Rigidbody>()){
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
        obj.transform.parent = holder.transform;
       obj.transform.localPosition = Vector3.zero;
       //if(obj.name == "Mug"){
           obj.transform.Rotate(30,0,0);
       //}
       
    }

    void release(){
        obj.transform.SetParent(null);
        if(obj.GetComponent<Rigidbody>()){
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
