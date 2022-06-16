using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startAudioSource : MonoBehaviour
{
    public List<GameObject> obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAudio(int i){
        obj[i].GetComponent<AudioSource>().Play();
    }

    public void stopAudio(int i){
        obj[i].GetComponent<AudioSource>().Stop();
    }
}
