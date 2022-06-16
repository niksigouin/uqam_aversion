using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeatMap;

public class DemoSceneManager : MonoBehaviour {

	public Recorder recorder;
	public Visualizer visualizer;

	public void GetData(){
        recorder.Stop();
		//visualizer.Show(recorder.GetData(), 0);
	}
}
