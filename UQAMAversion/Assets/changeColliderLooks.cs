using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeColliderLooks : MonoBehaviour
{

    public GameObject manag;
    public Collider NextfaceColl;


    public Collider NextchestColl;
    
    public Collider NexthandRIGHTColl;

    public Collider NexthandLEFTColl;

    public Collider NextHipsColl;

    public Collider NextfootRIGHTColl;

    public Collider NextfootLEFTColl;
    // Start is called before the first frame update
    void Start()
    {
         manag.GetComponent<lookDetect>().faceColl =  NextfaceColl;


         manag.GetComponent<lookDetect>().chestColl = NextchestColl;   

         manag.GetComponent<lookDetect>().handRIGHTColl =NexthandRIGHTColl;

         manag.GetComponent<lookDetect>().handLEFTColl =NexthandLEFTColl;

         manag.GetComponent<lookDetect>().HipsColl =NextHipsColl;

         manag.GetComponent<lookDetect>().footRIGHTColl =NextfootRIGHTColl;

         manag.GetComponent<lookDetect>().footLEFTColl =NextfootLEFTColl;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
