using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeatMap;

public class nextScene : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> sceneList = new List<GameObject>();
    [SerializeField] private VRInputController inputController;
    public GameObject BlackPannel;
    public Light lumiere;

    public Color PannelColor;

    private Color lerC = new Color(0,0,0,0);

    public float lumIntensity;

    public int sceneIndex = 0;

    public string sceneName = "";


    float timerMax = 15f;
    public float currentTime = 0;
    public bool timerOn = false;
	public int fractionTemps = 1;

    public bool change = false;

    public bool up = false;

    public int oldScene;

    public float ratio = 0;

    public Recorder recorderHeatMAP;

    public Visualizer viewerHeatMAP;

    public bool recordOn = false;

    public bool goRecord = false;

    public bool end = false;

    public bool sceneActive = true;

    
    void Start()
    {

        oldScene = sceneIndex;
        PannelColor = BlackPannel.GetComponent<MeshRenderer>().material.color;
        lumIntensity = lumiere.intensity;

        lumiere.intensity = 0;
        BlackPannel.GetComponent<MeshRenderer>().material.color = lerC;
        sceneName = sceneList[sceneIndex].name;

    
    }

    // Update is called once per frame
    void Update()
    {
        if(goRecord == true && recordOn == false){
            recordOn = true;
            goRecord = false;
            // recorderHeatMAP.Record();
            this.GetComponent<lookDetect>().look = true;
        }

        if(change == true && sceneIndex < 7){
            
            BlackPannel.SetActive(true);
            BlackPannel.GetComponent<MeshRenderer>().material.color = Color.Lerp(lerC, PannelColor, ratio);
            lumiere.intensity = Mathf.Lerp(0, lumIntensity, ratio);
            ratio += Time.deltaTime/fractionTemps;
            if( BlackPannel.GetComponent<MeshRenderer>().material.color == PannelColor && lumiere.intensity == lumIntensity){
                change = false;
                ratio = 0;
                sceneIndex++;
                timerOn = true;
            }
            // inputController.ToggleHands(false);
            inputController.TogglePlayerGravity(false);
            inputController.TogglePlayerMouvement(false);
        }

        if(sceneIndex != oldScene && sceneActive == true){
            sceneList[oldScene].SetActive(false);
            sceneActive = false;
            oldScene = sceneIndex;

        }

        if(timerOn){
            currentTime += Time.deltaTime;
            if(currentTime >= timerMax){
               currentTime = 0;
                timerOn = false;
                up = true;
            }
        }

        if(up == true){
            BlackPannel.GetComponent<MeshRenderer>().material.color = Color.Lerp(PannelColor, lerC, ratio);
            lumiere.intensity = Mathf.Lerp(lumIntensity, 0, ratio);
            ratio += Time.deltaTime/fractionTemps;
            
            if(ratio > 0.6f && sceneActive == false){
                sceneActive = true;
                sceneList[sceneIndex].SetActive(true);
                sceneName = sceneList[sceneIndex].name;
                
            }

      
            if( BlackPannel.GetComponent<MeshRenderer>().material.color == lerC && lumiere.intensity == 0){
                ratio = 0;
			    BlackPannel.SetActive(false);
                up = false;
                
                // TOGGLE PLAYER ACTIONS AND MOUVEMENTS AFTER LOADING IS COMPLET
                inputController.ToggleHands(true);
                inputController.TogglePlayerGravity(true);
                inputController.TogglePlayerMouvement(true);
            }
        }

    }

    public void next(){
        if(sceneIndex == 7){
            // viewerHeatMAP.Compile();
            sceneList[sceneIndex].SetActive(false);
            sceneIndex++;
        }

        else{
            change = true;
            if(sceneIndex > 0){
                // recorderHeatMAP.Stop();
                // viewerHeatMAP.Stock(recorderHeatMAP.GetData());
                // recorderHeatMAP.Reset();
                recordOn = false;
                goRecord = false;
            }
            this.GetComponent<lookDetect>().look = false;
            this.GetComponent<lookDetect>().writeData("**********------------"+ sceneList[sceneIndex+1].name +"------------**********");

        }
        
    }

  
}
