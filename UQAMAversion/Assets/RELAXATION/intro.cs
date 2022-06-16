using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro : MonoBehaviour
{

    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (audio.time >= 21)
        {
            this.GetComponent<relaxation>().respiration = true;
        }
    }
}
