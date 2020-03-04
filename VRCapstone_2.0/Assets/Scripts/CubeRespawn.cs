using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRespawn : MonoBehaviour
{
    private Game_Manager gm;

    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            Destroy(other.gameObject);
            gm.SpawnBall();
        }
    }
}
