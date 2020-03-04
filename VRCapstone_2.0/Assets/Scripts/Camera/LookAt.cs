using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
   private GameObject myObject, myObject2;
    private Game_Manager gm;
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }
    void Update()
    {
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))//, 10))//, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Interactable" && gm.hasGrabbed && hit.collider.gameObject.GetComponent<Alcohol_Stats>() != null) // looking at bottle
            {
                myObject = hit.collider.gameObject;
                myObject.GetComponent<Alcohol_Stats>().sizeUI.enabled = true;
            }
            else ExitHover();
            if (hit.collider.gameObject.tag == "Watch") //looking at watch
            {
                myObject2 = hit.collider.gameObject;
                myObject2.GetComponent<Watch>().hover = true;
            }
            else
            {
                if (myObject2 != null) myObject2.GetComponent<Watch>().hover = false;
                myObject2 = null;
            }
        }
        else Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
    }
    public void ExitHover()
    {
        if(myObject != null) myObject.GetComponent<Alcohol_Stats>().sizeUI.enabled = false;
        myObject = null;
    }
}
