using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alcohol_Stats : MonoBehaviour
{
    [System.Serializable]
    public class Stats
    {
        public string infoName;
        public string info;
    }
    [Header("Alcohol Stats")]
    public string title;
    public GameObject bottleObj, liquidObj, cupObj;
    public Text sizeUI, alcoholOunces, alcoholML;
    public bool grabbed, hover, isCenter;
    public Stats[] stats;
    private UnitySimpleLiquid.LiquidContainer liquidSize;
    public float curOunces, bottleSize = 0.354882f;

    public void Start()
    {
        liquidSize = this.GetComponent<UnitySimpleLiquid.LiquidContainer>();
    }
    public void Update()
    {
        curOunces = (liquidSize.FillAmountPercent * bottleSize) * 33.8140226f;
        sizeUI.text = curOunces.ToString("F1") + " oz";
    }
    public void GrabObject(bool grabbedObj)
    {
        grabbed = grabbedObj;
        liquidObj.SetActive(true);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>().hasGrabbed = true;
    }
    public void UseGravity()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
    }
}
