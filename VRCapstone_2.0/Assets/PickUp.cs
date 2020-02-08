using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PickUp : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticSignal;
    public AudioSource aus;
    public AudioClip myClip;

    public SteamVR_Action_Boolean trackpadAction;
    public bool leftHand, grab;

    void Update()
    {
        
            if (trackpadAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                grab = true;
                Haptic(.1f, 10, 20, SteamVR_Input_Sources.RightHand);
            }
            if (trackpadAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                grab = true;
                Haptic(.1f, 10, 20, SteamVR_Input_Sources.LeftHand);
            }
     
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Interactable")
        {
            aus.PlayOneShot(myClip);
            if (leftHand) Haptic(.1f, 10, 75, SteamVR_Input_Sources.LeftHand);
            else Haptic(.1f, 10, 75, SteamVR_Input_Sources.RightHand);
        }
    }
    public void Haptic(float duration, float frequency,float amplitude, SteamVR_Input_Sources source)
    {
        hapticSignal.Execute(0, duration, frequency, amplitude, source);
    }

}
