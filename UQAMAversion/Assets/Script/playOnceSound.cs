using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playOnceSound : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AudioSource> audioPlayer = new List<AudioSource>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playAudioSourceOnce(int index){
        audioPlayer[index].Play();
    }
}
