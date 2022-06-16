using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteMug : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mug;
    void Start()
    {
        Destroy(mug);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
