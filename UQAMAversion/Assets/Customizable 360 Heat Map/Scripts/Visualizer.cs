using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

namespace HeatMap
{
	public class Visualizer : MonoBehaviour {
        
        public const string SAVE_TEXTURE_PATH = "/heatmapAversion/";

		public Texture2D defaultTexture;
        public Texture2D sourceTexture;

        [Range(0,1)]
        public float heapmapTransparency;
        [Header("If Save As PNG is enabled, the generated texture will be saved in the Customizable 360 Heat Map/HeatmapTextures folder")]
        public bool saveAsPNG;
        [Header("If you want to override files with the same name check the box below, else the filename will increment with: ...(1), ...(2), etc.")]
        public bool overrideFileWithSameName;
        public string savedPNGName;
        public Color[] colorProgress;
        public GameObject manager;
        private Material material;
        private MeshRenderer mesh;
        private float[] sourceArray;
        private bool isEnabled;

        public List<float[]> heatMapsDatas = new List<float[]>();


        public List<Texture2D> texturesList = new List<Texture2D>();

        public GameObject fondCaptureCompile;

        public List<GameObject> cams4Capture;

        public GameObject foveGO;

        public GameObject lastScene;


        private bool GoToCaptures = false;

		void Start(){
            //Give error if we have to colors to blend
            if(colorProgress.Length < 2)
            {
                Debug.LogError("The variable 'colorProgress' must have a minimum of 2 colors in order to blend the values");
                enabled = false;
                return;
            }
            if(defaultTexture == null)
            {
                Debug.LogError("You need to assign the default texture, try using(/Assets/Vr");
            }

            mesh = GetComponent<MeshRenderer>();
            isEnabled = mesh.enabled;
			material = GetComponent<Renderer>().material;
			material.mainTexture = defaultTexture;

            // Get color array for the source texture
            Color[] cols = sourceTexture.GetPixels();
            sourceArray = new float[cols.Length];
            for(int i = 0; i < cols.Length; i++)
            {
                sourceArray[i] = cols[i].a;
            }
        }

        void Update(){

          /*  if(GoToCaptures == true && !EditorApplication.isCompiling){
                 GoToCaptures = false;
                StartCoroutine(Captures());
               
            }*/
        }

		/*IEnumerator*/void Show(float[] data, int iNoScene){
//            print(data.Length);
			// Create alpha texture
			int size = (int)Mathf.Sqrt(data.Length);
			Texture2D tex = new Texture2D(size , size);

            //Add spread to the values
            float[] newData = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] > 0)
                {
                    //Loop trough all neighboring pixels and add spread
                    int spread = (int)Mathf.Sqrt(sourceArray.Length) / 2;
                    for (int y = -spread; y < spread; y++)
                    {
                        for (int x = -spread; x < spread; x++)
                        {
                            float sourcePixelIndex = sourceArray[(x + spread) + ((y + spread) * (2 * spread))];
                            int pixelIndexToChange = i + ((int)size * y) + x;

                            if (pixelIndexToChange > 0 && newData != null && newData.Length > pixelIndexToChange)
                            {
                                newData[pixelIndexToChange] += (float)(data[i] * sourcePixelIndex);
                            }
                        }
                    }
                }
            }

            float max = 0;
            float min = 0;

			// Paint texture according to the data
			for(int i = 0 ; i < newData.Length ; i++){
				if(newData[i] > max) max = newData[i];
			}
            
            // Paint texture according to the data
            for (int i = 0 ; i < newData.Length ; i++){
				
				int x = i % size;
				int y = (i / size);

				float a = 0;

				if(newData[i] > 0){
					a = (float)(newData[i] - min)/(max-min);
				}

                //Create color blend
                float progress = (colorProgress.Length - 1) * a;
                int flooredProgress = Mathf.FloorToInt(progress);
                int ceiledProgress = Mathf.CeilToInt(progress);

                Color col = colorProgress[0];
                Color minC = colorProgress[flooredProgress];
                Color maxC = colorProgress[ceiledProgress];
                float newA = progress - flooredProgress;
                col = Color.Lerp(minC, maxC, a);
                col.a = heapmapTransparency;

                tex.SetPixel(x,y,col);
            }

            //tex.Apply();
            //material.mainTexture = tex;
            //texturesList.Add(tex);
            

            if (saveAsPNG)
            {
                Texture2D savedTex = new Texture2D(tex.width, tex.height);
                savedTex.SetPixels32(tex.GetPixels32());
                savedTex.Apply(false);
                string fileName = savedPNGName;
                if (!overrideFileWithSameName)
                {
                    fileName = RecursiveFileNameCheck(Application.dataPath + SAVE_TEXTURE_PATH, fileName + "_"+ manager.GetComponent<nextScene>().sceneList[iNoScene].name);
                }
                File.WriteAllBytes(Application.dataPath + SAVE_TEXTURE_PATH + fileName + ".png", savedTex.EncodeToPNG());
                
            }

            //yield return null;
            
		}

		public void Reset(){
			material.mainTexture = defaultTexture;
		}

        public void Stock(float[] data){
            heatMapsDatas.Add(data);
        }

        public void Compile(){
            lastScene.SetActive(false);

            int sceneIndex = 1;
            foreach (var data in heatMapsDatas)
            {
                Show(data, sceneIndex);
                //heatMapsDatas[sceneIndex-1] = null;
                sceneIndex++;
                
            }
            
            GoToCaptures = true;
        }

        IEnumerator Captures(){
            fondCaptureCompile.SetActive(true);
            foveGO.SetActive(false);

            int camIndex = 0;
            foreach(var tex in texturesList){
                cams4Capture[camIndex].SetActive(true);
                material.mainTexture = tex;

                yield return new WaitForSeconds(5);

                ScreenCapture.CaptureScreenshot("2DheatMap_scene"+(camIndex+1)+".png");
                cams4Capture[camIndex].SetActive(false);
                camIndex++;
                
            }
        }

        public void ToggleVisuals(bool b)
        {
            isEnabled = b;
            mesh.enabled = b;
        }

        public void ToggleVisuals()
        {
            isEnabled = !isEnabled;
            mesh.enabled = isEnabled;
        }

        private string RecursiveFileNameCheck(string path, string stringToCheck, int rounds = 0)
        {
            string newString = stringToCheck;
            if(rounds > 0)
            {
                newString = stringToCheck + "(" + rounds.ToString() + ")";
            }
            
            if (File.Exists(path + newString + ".png"))
            {
                return RecursiveFileNameCheck(path, stringToCheck, rounds + 1);
            }
            else
            {
                print(newString);
                return newString;
            }
        }
	}
}