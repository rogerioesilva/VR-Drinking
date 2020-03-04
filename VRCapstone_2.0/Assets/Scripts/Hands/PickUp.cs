using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PickUp : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticSignal;
    public bool leftHand, hover;

    [Header("Audio Stats")]
    public AudioSource aus;
    public AudioClip myClip;

    //public SteamVR_Action_Boolean trackpadAction;

    void Update()
    {

        /*if (trackpadAction.GetStateDown(SteamVR_Input_Sources.RightHand) )
            {
           // myTime += Time.deltaTime;
                grab = true;
               Haptic(.5f, 200, 20, SteamVR_Input_Sources.RightHand);
            }
            if (trackpadAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                grab = true;
                Haptic(Time.deltaTime, 100, 20, SteamVR_Input_Sources.LeftHand);
            }*/
     
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "PickUp" || other.gameObject.tag == "Interactable" || other.gameObject.tag == "Continous")
        {
            aus.PlayOneShot(myClip);
            if (leftHand) Haptic(.1f, 10, 75, SteamVR_Input_Sources.LeftHand);
            else Haptic(.1f, 10, 75, SteamVR_Input_Sources.RightHand);

            if (other.gameObject.tag == "Interactable") other.gameObject.GetComponent<Alcohol_Stats>().hover = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable") other.gameObject.GetComponent<Alcohol_Stats>().hover = false;
    }

    public void Haptic(float duration, float frequency,float amplitude, SteamVR_Input_Sources source)
    {
        hapticSignal.Execute(0, duration, frequency, amplitude, source);
    }
}
