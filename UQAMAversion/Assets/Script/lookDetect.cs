using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
using Fove;
using System.IO;
using UnityEngine.UI;
//using HeatMap;

public class lookDetect : MonoBehaviour
{
    public FoveInterface Fove;
    public GameObject rig;
    public Collider faceColl;
    public bool face;
     public float timerface;
    public bool timerfaceON;

    public Collider chestColl;
    public bool chest;
     public float timerchest;
    public bool timerchestON;

    public Collider handRIGHTColl;
    public bool handRIGHT;
    public float timerhandR;
    public bool timerhandRON;

    public Collider handLEFTColl;
    public bool handLEFT;
    public float timerhandL;
    public bool timerhandLON;

    public Collider HipsColl;
    public bool Hips;
    public float timerhips;
    public bool timerhipsON;

    public Collider footRIGHTColl;
    public bool footRIGHT;
    public float timerfootR;
    public bool timerfootRON;

    public Collider footLEFTColl;
    public bool footLEFT;
    public float timerfootL;
    public bool timerfootLON;

    public bool other;

    public float timerNone = 0;
    public bool timerNoneOn;

    public string activeLook;

    string testFileName = "LookTimer/AversionLookTimers.txt";
    string fileName     = "LookTimer/AversionLookTimers";

    public GameObject blobR;

    public GameObject blobL;

    public GameObject visualLeftBlob;

    public GameObject visualRightBlob;

    private float precisLook = 1.1f;

    private float transitTimer = 0;

    public float tansitSpeed = 10f;

    //public Recorder recorderHeatMAP;

    //public Visualizer viewerHeatMAP;

    public bool recordOn = false;

    public bool end = false;

    public bool look = false;

    public float generalTimer = 0;

    public bool genTimOn = false;

    public Text timeText;

    public GameObject whitePannel;


   
    // Start is called before the first frame update
    void Start()
    {
        createFile();

    

      
        //this.GetComponent<nextScene>().sceneIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = generalTimer.ToString();

        if(look == false && genTimOn == true){
            genTimOn = false;
            generalTimer = 0;
        }

        if(this.GetComponent<nextScene>().sceneIndex > 0 && this.GetComponent<nextScene>().sceneIndex < 7 && look == true){
           
           if(genTimOn == false){
               genTimOn = true;
               whitePannel.SetActive(true);
           }

           

           if(genTimOn == true){
               generalTimer += Time.deltaTime;
           }

           if(generalTimer > 0.5f){
               whitePannel.SetActive(false);
           }


                if(!timerfaceON && !timerfootLON && !timerfootRON && !timerhandLON && !timerhandRON && !timerhipsON && !timerchestON && timerNoneOn == false)
                {
                    timerNoneOn = true;
                    Debug.Log("regard non-defini");
                    activeLook = "non-defini";
                }

                if(timerNoneOn == true){
                    timerNone += Time.deltaTime;
                }

                if(timerfaceON || timerfootLON || timerfootRON || timerhandLON || timerhandRON || timerhipsON || timerchestON){
                    if(timerNoneOn == true){
                        writeData("0, " + timerNone + " / " +generalTimer);
                        timerNoneOn = false;
                        timerNone = 0;
                    }
                }

                face = Fove.Gazecast(faceColl);
                if(face){
                    activeLook = "visage";

                    if(timerfaceON == false){
                        timerfaceON = true;
                        Debug.Log("visage");
                    }

                    timerface += Time.deltaTime;
                }
        
                if(!face && timerfaceON == true){ 
                    writeData("1, " + timerface + " / " +generalTimer);
                        timerfaceON = false;
                        timerface = 0;
                        Debug.Log("sorti du visage");
                }
                


                chest = Fove.Gazecast(chestColl);
                if(chest){
                    activeLook = "torse";
                    if(timerchestON == false){
                        timerchestON = true;
                        Debug.Log("torse");
                    }

                    timerchest += Time.deltaTime;
                }  
                if(!chest && timerchestON == true){
                    writeData("2, " + timerchest + " / " +generalTimer);
                        timerchestON = false;
                        timerchest = 0;
                        Debug.Log("sorti du torse");
                }
                

                handRIGHT = Fove.Gazecast(handRIGHTColl);
                if(handRIGHT){
                    activeLook = "mainDroite";
                    if(timerhandRON == false){
                        timerhandRON = true;
                        Debug.Log("main droite");
                    }

                    timerhandR += Time.deltaTime;
                } 
                if(!handRIGHT && timerhandRON == true){ 
                    writeData("3, " + timerhandR + " / " +generalTimer);
                        timerhandRON = false;
                        timerhandR = 0;
                        Debug.Log("sorti de la main droite");
                }

                handLEFT = Fove.Gazecast(handLEFTColl);
                if(handLEFT){
                    activeLook = "mainGauche";
                    if(timerhandLON == false){
                        timerhandLON = true;
                        Debug.Log("main gauche");
                    }
                    timerhandL += Time.deltaTime;
                }
                if(!handLEFT && timerhandLON == true){ 
                        writeData("4, " + timerhandL + " / " +generalTimer);
                        timerhandLON = false;
                        timerhandL = 0;
                        Debug.Log("sorti de la main gauche");
                }


                Hips = Fove.Gazecast(HipsColl);
                if(Hips){
                    activeLook = "Hanche";
                    if(timerhipsON == false){
                        timerhipsON = true;
                        Debug.Log("hanches");
                    }
                    timerhips += Time.deltaTime;
                }
                if(!Hips && timerhipsON == true){ 
                        writeData("5, " + timerhips + " / " +generalTimer);
                        timerhipsON = false;
                        timerhips = 0;
                        Debug.Log("sorti des hanches");
                }


                footRIGHT = Fove.Gazecast(footRIGHTColl);
                if(footRIGHT){
                    activeLook = "piedDroit";
                    if(timerfootRON == false){
                        timerfootRON = true;
                        Debug.Log("pied droit");
                    }
                    timerfootR += Time.deltaTime;
                }
                if(!footRIGHT && timerfootRON == true){ 
                    writeData("6, " + timerfootR + " / " +generalTimer);
                        timerfootRON = false;
                        timerfootR = 0;
                        Debug.Log("sorti du pied droit");
                }

                footLEFT = Fove.Gazecast(footLEFTColl);
                if(footLEFT){
                    activeLook = "piedGauche";
                    if(timerfootLON == false){
                        timerfootLON = true;
                        Debug.Log("pied gauche");
                    }
                    timerfootL += Time.deltaTime;
                }
                if(!footLEFT && timerfootLON == true){ 
                    writeData("7, " + timerfootL + " / " +generalTimer);
                        timerfootLON = false;
                        timerfootL = 0;
                        Debug.Log("sorti du pied gauche");
                }

            
                

                //Debug.Log(activeLook);


                

               
            }

            transitTimerCount();
                blobToPosLookR();
                blobToPosLookL();

            /*TEST HEATMAP
            if(this.GetComponent<nextScene>().sceneIndex == 0 && recordOn == false){
                recordOn = true;
                recorderHeatMAP.Record();
            }

           if(this.GetComponent<nextScene>().sceneIndex == 1 && recordOn == true){
                recordOn = false;
                recorderHeatMAP.Stop();
                GetHeatMapData();
            }*/
/*
            if(this.GetComponent<nextScene>().sceneIndex == 1 && recordOn == false){
                recordOn = true;
                recorderHeatMAP.Record();
            }

            if(this.GetComponent<nextScene>().sceneIndex == 7 && recordOn == true){
                recordOn = false;
                recorderHeatMAP.Stop();
            }

           if(this.GetComponent<nextScene>().sceneIndex == 8 && end == false){
                    end = true;
                    GetHeatMapData();
                } 
*/
        
    }


    public void createFile(){

        

                int counter = 1;
				while (File.Exists(testFileName))
				{
					testFileName = fileName + "_" + (counter++) + ".txt"; // e.g., "results_12.csv"
				}
			
			    fileName = testFileName;

                //create a .txt file if file does not exist
                File.CreateText(fileName);
                //Now add in your data
            
    }
    public void blobToPosLookR(){
        
        blobR.transform.localPosition = new Vector3(FoveManager.GetRightEyeVector(false).x * precisLook, FoveManager.GetRightEyeVector(false).y * precisLook, 1);
            
        visualRightBlob.transform.localPosition = Vector3.Lerp(visualRightBlob.transform.localPosition, blobR.transform.localPosition, transitTimer);
       
    }

    public void blobToPosLookL(){
        blobL.transform.localPosition = new Vector3(FoveManager.GetLeftEyeVector(false).x * precisLook, FoveManager.GetLeftEyeVector(false).y * precisLook, 1);

        visualLeftBlob.transform.localPosition = Vector3.Lerp(visualLeftBlob.transform.localPosition, blobL.transform.localPosition, transitTimer);
    }

    public void transitTimerCount(){

        transitTimer += Time.deltaTime/tansitSpeed;

        if(transitTimer >=1){
            transitTimer = 0;
        }
    }


    public void writeAnimStart(string animName){
        var info = animName;

        info += " / " +generalTimer;

        writeData(info);
    }
    public void writeData(string data){
//        Debug.Log(fileName);
        StreamWriter writer = new StreamWriter(fileName, true);
        writer.WriteLine(data + "\n");
        writer.Close();
    }

    

   /* public void GetHeatMapData(){
		viewerHeatMAP.Show(recorderHeatMAP.GetData());
	}*/
}
