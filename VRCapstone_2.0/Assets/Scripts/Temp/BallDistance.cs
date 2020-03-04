using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDistance : MonoBehaviour
{
    public float ballDistance;
    private Game_Manager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        ballDistance = Vector3.Distance(GameObject.Find("Ball").gameObject.transform.position, this.transform.position);
        if (ballDistance > 1.1f)
        {
            GameObject.Find("Ball").name = "Old";
            gm.SpawnBall();
        }
    }
}
