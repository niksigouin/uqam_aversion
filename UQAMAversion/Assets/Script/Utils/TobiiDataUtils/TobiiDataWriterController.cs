using System;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace Script.Utils
{
    public class TobiiDataWriterController : MonoBehaviour
    {
        private static TobiiDataWriterController _instance;
        public static TobiiDataWriterController Instance { get { return _instance; } }
        
        private string path;
        private string fileName;
        private DateTime localTime;

        private bool fileCreated = false;


        public Action<global::PointOfInterest, float> POILook;
        public Action<global::PointOfInterest, float> POIGrab; 
        public Action<global::PointOfInterest, float> EroticZoneTouchEvent;
        public Action<string, CharacterAnimState> AnimationEvent;


        private void Start()
        {
            if (_instance != null && _instance != this) Destroy(this.gameObject);
            else _instance = this;
            
            path = Application.persistentDataPath + "/AversionData/";
            localTime = DateTime.Now;
            fileName = $"{path}{localTime.Date.ToString("yy-MM-dd")}_{localTime.Hour}-{localTime.Minute}-{localTime.Second}.txt";
            // Debug.Log(fileName);
            CreateFile();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            Instance.POILook += OnPOILook;
            Instance.POIGrab += OnPOIGrab;
            SceneManager.sceneLoaded += OnSceneChange;
            Instance.AnimationEvent += OnAnimationEvent;
            Instance.EroticZoneTouchEvent += OnEroticZoneTouch;
        }

        

        private void Unsubscribe()
        {
            Instance.POILook -= OnPOILook;
            Instance.POIGrab -= OnPOIGrab;
            SceneManager.sceneLoaded -= OnSceneChange;
            Instance.AnimationEvent -= OnAnimationEvent;
            Instance.EroticZoneTouchEvent -= OnEroticZoneTouch;
        }

        private void CreateFile()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.Log($"Created directory: {path}");
            }
            
            WriteHeader();
            // If directory of current day isn't created, create it, else write to said folder (dir)
            Debug.Log($"Created file at path: {fileName}");
            fileCreated = true;
        }
        
        private void OnPOILook(global::PointOfInterest POIindex, float lookTime)
        {
            string data = $"[LOOK], ({POIindex.ToString()}), {(int) POIindex}, {lookTime} / {Time.time}";
            WriteData(data);
            Debug.Log(data);
        }

        private void OnSceneChange(Scene sceneName, LoadSceneMode loadSceneMode)
        {
            string data = $"[SCENE CHANGE], ****** {sceneName.name} ******";
            WriteData(data);
            Debug.Log(data);
        }
        
        private void OnAnimationEvent(string animName, CharacterAnimState animState)
        {
            string data = $"[ANIMATION], {animName}, {animState.ToString()} / {Time.time}";
            Debug.Log(data);
            WriteData(data);
        }

        private void OnEroticZoneTouch(global::PointOfInterest POIindex, float touchTime)
        {
            string data = $"[TOUCH], ({POIindex.ToString()}), {(int) POIindex}, {touchTime} / {Time.time}";
            WriteData(data);
            Debug.Log(data);
        }

        private void OnPOIGrab(global::PointOfInterest POIindex, float grabTime)
        {
            string data = $"[GRAB], ({POIindex.ToString()}), {(int) POIindex}, {grabTime} / {Time.time}";
            WriteData(data);
        }

        private void WriteHeader()
        {
            WriteData($"DATE: {localTime.Date.ToString("yy/MM/dd")} {localTime.Hour}:{localTime.Minute}:{localTime.Second}");

            string indexs = "***** INDEX VALUES *****\n";
            // TODO: INCLUDE ALL ENUMS FOR POINTS OF INTEREST
            foreach (var POI in Enum.GetValues(typeof(global::PointOfInterest)))
            {
                indexs += $"{(int)POI} => {POI}\n";
            }

            indexs += "*******************";
            WriteData(indexs);
            OnSceneChange(SceneManager.GetSceneAt(0), LoadSceneMode.Single);
        }

        private void WriteData(string data)
        {
            // StartCoroutine(StreamWriteData(data));
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.WriteLine(data + "\n");
            writer.Close();
        }
    }
}

public enum PointOfInterest
{
    Autre = 0,
    Visage,
    Main,
    Penis,
    Vagin,
    Seins,
    Bouton_Orange,
    Bouton_Vert,
    Pomme,
    Sphere_relaxation
}