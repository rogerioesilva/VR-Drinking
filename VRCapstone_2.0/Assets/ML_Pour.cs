using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ML_Pour : MonoBehaviour
{
    public float curAlcohol, curAlcoholOunces;
    public float curBottle;
    public UnitySimpleLiquid.LiquidContainer liquidSize;
    public GameObject parent;
    public int counter;

    public void Start()
    {
        liquidSize = this.transform.parent.gameObject.GetComponent<UnitySimpleLiquid.LiquidContainer>();
        parent = this.transform.parent.gameObject;
        curBottle = parent.GetComponent<Alcohol_Stats>().bottleSize;
    }
    public void Update()
    {
        if (curAlcoholOunces >= Mathf.Ceil(curBottle * 33.8140226f))
        {
            curAlcoholOunces = Mathf.Ceil(curBottle * 33.8140226f);
            curAlcohol = curAlcoholOunces * 29.5735296f;
        }
    }
    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        // particles
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
       
        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
             //p.startColor = new Color32(255, 0, 0, 255);
            enter[i] = p;
           
        }
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
          //   p.startColor = new Color32(0, 255, 0, 255);
            exit[i] = p;
          

            //  Debug.Log("Inside3");
        }
        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
        counter++;
        if (counter >= 130)
        {
            curAlcoholOunces++; //((curBottle- (liquidSize.FillAmountPercent * curBottle)) * 33.8140226f);
            curAlcohol = (curAlcoholOunces * 29.5735296f);
            if (curAlcoholOunces >= Mathf.Ceil(curBottle * 33.8140226f))
            {
                curAlcoholOunces = Mathf.Ceil(curBottle * 33.8140226f);
                curAlcohol = curAlcoholOunces * 29.5735296f;
            }
            parent.GetComponent<Alcohol_Stats>().alcoholOunces.text = curAlcoholOunces.ToString("F2") + " oz";
            parent.GetComponent<Alcohol_Stats>().alcoholML.text = curAlcohol.ToString("F2") + " mL";
            Debug.Log(curAlcoholOunces);
            Debug.Log("Inside2");
            counter = 0;
        }
    }
}
