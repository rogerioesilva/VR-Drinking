using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject particleObj;
    public AudioSource aus;
    public AudioClip myClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            Destroy(other.gameObject);
            GameObject go = Instantiate(particleObj, other.transform.position, other.transform.rotation);
            aus.PlayOneShot(myClip);

        }
    }
}
