using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleInside : MonoBehaviour
{
    public Material inside, outside;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            other.gameObject.GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = inside;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            other.gameObject.GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = outside;
        }
    }
}
