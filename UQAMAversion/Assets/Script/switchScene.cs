using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScene : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject fondNoir;

    public bool on = true;

    public bool invisible = false;

    public float alpha = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(on == true && invisible == false){
           if(fondNoir.GetComponent<Renderer>().material.color.a>0){
              // fondNoir.GetComponent<Renderer>().material.SetAlpha(fondNoir.GetComponent<Renderer>().material.color.a - 0.1F);
           }
           else{
               invisible = true;
           }
            
        }

        if(on == false && invisible == true){

        }
    }
}
