using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    public Material normMaterial, hoverMaterial;
    public bool hover;
    public GameObject stats;

    public void Update()
    {
        if (hover)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = hoverMaterial;
            stats.SetActive(true);
        }
        else
        {
            stats.SetActive(false);
            this.gameObject.GetComponent<MeshRenderer>().material = normMaterial;
        }
    }
}
