using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawnLocation : MonoBehaviour
{
    [SerializeField] private bool useSpawnLocation = true;
    private void Start()
    {
        // yield return new WaitUntil(() => SceneChangeController.Instance != null);
        DoTeleport();
        
            // try with location and quaternion rotation
    }

    public void DoTeleport()
    {
        if (useSpawnLocation)
        {
            Debug.Log("CALLED PLAYER TELEPORTATION");
            // TODO: IF MALE NOT FIX UNCOMMENT
            // SceneChangeController.Instance.TeleportPlayerToSpawn(this.transform);
        }
    }
}
