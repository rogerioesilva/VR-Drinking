using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    [Header("Renderer")]
    public bool hover;
    public Material normMaterial, hoverMaterial;

    [Header("Stats")]
    public GameObject stats;
    public Animator anim;

    [Header("Audio")]
    public AudioSource aus;
    public AudioClip mySound;
    private bool played;

    public void Update()
    {
        if (hover)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = hoverMaterial;
            stats.SetActive(true);
            
            //MISC
            if (!played)
            {
                aus.PlayOneShot(mySound);
                played = true;
                anim.SetBool("Open", true);
            }
        }
        else
        {
            anim.SetBool("Open", false);
            if (played)
            {
                aus.PlayOneShot(mySound);
                played = false;
            }
          //  stats.SetActive(false);
            this.gameObject.GetComponent<MeshRenderer>().material = normMaterial;
        }
    }
}
