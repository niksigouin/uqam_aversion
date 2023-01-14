using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;

public class VRInputController : MonoBehaviour
{
    [Header("OBJECTS")]
    [SerializeField] private List<GameObject> hands;

    [Header("OPTIONS")] 
    [SerializeField] private GameObject playerController;

    [SerializeField] private bool initialPlayerControl = false;
    
    // Start is called before the first frame update
    void Start()
    {
        InitPlayerController(initialPlayerControl);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitPlayerController(bool state)
    {
        ToggleHands(state);
        // TogglePlayerGravity(state);
        TogglePlayerMouvement(state);
    }

    public void TogglePlayerGravity(bool state)
    {
        // playerController.GetComponent<PlayerGravity>().GravityEnabled = state;
    }
        
    public void TogglePlayerMouvement(bool state)
    {
        playerController.GetComponent<SmoothLocomotion>().AllowInput = state;
        playerController.GetComponent<PlayerTeleport>().enabled = state;
    }

    public void ToggleHands(bool state)
    {
        Debug.LogWarning($"Setting hands to || {state}");
        foreach (var hand in hands)
        {
            hand.SetActive(state);
        }
    }
}
