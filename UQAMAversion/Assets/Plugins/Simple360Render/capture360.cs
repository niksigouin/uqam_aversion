using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class capture360 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject manager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown("space"))
        {
          //File.WriteAllBytes("capture.jpg",I360Render.Capture(8192, true, Camera.main, true ));
          ScreenCapture.CaptureScreenshot("test.png");
        }

        

    }

     
}
