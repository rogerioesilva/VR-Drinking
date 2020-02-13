using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRespawn : MonoBehaviour
{
    public Transform spawnPt;
    public GameObject spawnObj;
    public GameObject particleObj;

    public AudioSource aus;
    public AudioClip myClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            aus.PlayOneShot(myClip);
            Destroy(other.gameObject);
            GameObject go = Instantiate(spawnObj, spawnPt.transform.position, Quaternion.identity);
            GameObject go2 = Instantiate(particleObj, other.transform.position, Quaternion.identity);
        //    GameObject go3 = Instantiate(particleObj, spawnPt.transform.position, Quaternion.identity);
        }
    }
}
