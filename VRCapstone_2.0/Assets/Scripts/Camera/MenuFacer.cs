using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFacer : MonoBehaviour
{
    private Transform target;
    public string targetName = "MainCamera";

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetName).GetComponent<Transform>();
    }
    void Update()
    {
        if (target != null) transform.LookAt(2*transform.position - target.position);
    }
}