using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Unity;
using Fove;

namespace HeatMap
{
	public class Recorder : MonoBehaviour {

		public enum TextureSize { _64 = 64 , _128 = 128 , _256 = 256, _512 = 512, _1024 = 1024, _2048 = 2048, _4096 = 4096, _8192 = 8192 }

        [SerializeField] TextureSize textureSize;

		private float[] data;
		private Material material;

		private bool _isRecording = false;
		private Camera mainCam;

		public LayerMask raycastLayer;

		private float precisLook = 1.1f;

		public bool isRecording{
			get {
				return _isRecording;
			}
		}

		// Use this for initialization
		void Start () {
			// Set the texture to minimum if it is not yet set in the editor
			if(textureSize == 0) textureSize = TextureSize._64;

			// Create the data array to store the hear map data
			ResetData();

			// Get the material
			material = GetComponent<Renderer>().material;

			// Create empty texture with the selected size
			Texture2D tex = new Texture2D((int)textureSize , (int)textureSize);

			// Assign texture to material
			material.mainTexture = tex;

			// Get the main camera for later use
			mainCam = Camera.main;

			_isRecording = false;

		}
		Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
		{
			// Find vectors corresponding to two of the sides of the triangle.
			Vector3 side1 = b - a;
			Vector3 side2 = c - a;

			// Cross the vectors to get a perpendicular vector, then normalize it.
			return Vector3.Cross(side1, side2).normalized;
		}
		// Update is called once per frame
		void Update () {
			if(_isRecording){	
				
				RaycastHit hit;
				var halfwayVector = (FoveManager.GetRightEyeVector(false) + FoveManager.GetLeftEyeVector(false)).normalized;
				//new Vector3(FoveManager.GetRightEyeVector(false).x * precisLook, FoveManager.GetRightEyeVector(false).y * precisLook, 1)
				if(Physics.Raycast(mainCam.transform.position , halfwayVector , out hit, raycastLayer)){

                    // Calculate the index number in the data array
                    int index = (int)((int)textureSize * hit.textureCoord.x) + ((int)textureSize * (int)((int)textureSize * hit.textureCoord.y));

                    // Increase data array element by 1.
                    if (data != null && data.Length > index){
						data[index] += 1 * Time.deltaTime;
					}
				}
			}
		}

		/*
			Starts recording the heatmap data
		*/
		public void Record(){
			if(_isRecording){
				Debug.LogWarning("Already recording.");
				return;
			}

			if(data == null || data.Length <= 0){
				Debug.LogError("Recorder is not properly initialised. Make sure you select the texture size in the editor.");
				return;
			}
			
		//	Debug.Log("Starts recording heat map.");
			_isRecording = true;
		}

		/*
			Stops recording the heatmap data. Does not clear the current data
		*/
		public void Stop(){
			if(_isRecording) {
	//			Debug.Log("Stops recording heat map.");
				_isRecording = false;
			}
		}

		/*
			Clears the data that was recorded so far
		*/
		public void Reset(){
//			Debug.Log("Clears heat map data.");
			Stop();
			ResetData();
		}

		/*
			If recording stops recording
			Returns the data as an array of integers
		 */
		public float[] GetData(){
			Stop();
            return data;
            //return coordData;
		}

		private void ResetData(){
			int size = (int)textureSize;
			data = new float[(size * size)];
		}
	}
}