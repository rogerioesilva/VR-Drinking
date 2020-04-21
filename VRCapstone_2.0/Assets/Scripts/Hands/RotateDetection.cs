using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDetection : MonoBehaviour
{
    [Header("Stats")]
    public SpawnMenu spawnMenu;
    public Quaternion handRot, tempRotation;

    [HideInInspector] public int curIndex;
    [HideInInspector] public bool inside, isReady;

    [Header("Audio")]
    public AudioSource aus;
    public AudioClip mySound;

    void LateUpdate()
    {
        handRot = this.GetComponent<Rigidbody>().rotation;

        if (inside)
        {
            if (handRot.y < -0.1f && tempRotation.y >= 0) //move right
            {
                if (!spawnMenu.moving)
                {
                    aus.PlayOneShot(mySound);
                    spawnMenu.moving = true; //rotate menu
                }
                tempRotation = handRot;
            }
            if (handRot.y >= 0) tempRotation = Quaternion.Euler(this.transform.rotation.x, 0, this.transform.rotation.z);
        }
        else tempRotation = handRot;

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bottle") inside = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bottle") inside = false;
    }
}
