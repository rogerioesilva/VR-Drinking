using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintColor : MonoBehaviour
{
    public Material sprayColor;
    private Game_Manager gm;

    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            //SAVE CURRENT MATERIAL
            gm.curMaterial = sprayColor;
            gm.curMaterial.color = sprayColor.color;
            gm.curMaterial.name = sprayColor.name;

            //SET NEW MATERIAL
            other.GetComponent<MeshRenderer>().materials[0] = sprayColor;
            other.GetComponent<MeshRenderer>().materials[0].color = sprayColor.color;
            other.GetComponent<MeshRenderer>().materials[0].name = sprayColor.name;
            other.GetComponent<TrailRenderer>().startColor = sprayColor.color;
            other.GetComponent<TrailRenderer>().endColor = sprayColor.color;
        }
    }
}
