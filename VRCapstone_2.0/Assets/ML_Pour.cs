using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ML_Pour : MonoBehaviour
{
    [Header("Measurements")]
    public int counter;
    public float curAlcohol, curAlcoholOunces, curBottle;

    private UnitySimpleLiquid.LiquidContainer liquidSize;
    private GameObject parent;


    public void Start()
    {
      //  liquidSize = this.transform.parent.gameObject.GetComponent<UnitySimpleLiquid.LiquidContainer>();
        parent = this.transform.parent.gameObject;
        curBottle = parent.GetComponent<Alcohol_Stats>().bottleSize;
        liquidSize = parent.GetComponent<UnitySimpleLiquid.LiquidContainer>();
    }
    public void Update() //create limits
    {
        if (curAlcoholOunces >= Mathf.Ceil(curBottle * 33.8140226f))
        {
            //curAlcoholOunces = Mathf.Ceil(curBottle * 33.8140226f);
            //curAlcohol = curAlcoholOunces * 29.5735296f;
        }
    }
    private void OnParticleTrigger() // is drinking
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        // get
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
       
        // iterate
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            enter[i] = p;
        }
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = exit[i];
            exit[i] = p;
        }

        // set
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

        //drinking
        counter++;
        if (counter >= 130)
        {
            curAlcoholOunces++; 
            curAlcohol = (curAlcoholOunces * 29.5735296f);
            if (curAlcoholOunces >= Mathf.Ceil(curBottle * 33.8140226f))
            {
                curAlcoholOunces = Mathf.Ceil(curBottle * 33.8140226f);
                curAlcohol = curAlcoholOunces * 29.5735296f;
            }
            parent.GetComponent<Alcohol_Stats>().ouncesText.text = curAlcoholOunces.ToString("F2") + " oz";
            parent.GetComponent<Alcohol_Stats>().mLText.text = curAlcohol.ToString("F2") + " mL";
            counter = 0;
        }
    }
}
