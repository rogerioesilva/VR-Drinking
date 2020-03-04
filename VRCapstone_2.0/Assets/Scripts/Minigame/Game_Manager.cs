using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    [Header("Color Stats")]
    public List<Material> colors;
    public List<int> setOrder;
    private int curColor, curIndex;

    [Header("Ball Stats")]
    public Transform spawnPt;
    public GameObject spawnObj, explosion;
    public AudioSource aus;
    public AudioClip respawnBall, shotMade;
    public Material hoopColor, curMaterial;

    [Header("Level Complete")]
    public GameObject uiComplete;
    public AudioClip completedSound;

    [Header("Timer")]
    private float score, highScore;
    public Text curTime, scoreText, highScoreText;


    [Header("Error")]
    public AudioClip errorSound;
    public GameObject errorUI;
    /////////////////////////////////////////////
    [Header("Alcohol Stats")]
    public bool hasGrabbed;
    public GameObject miniGame, alcoholMenu;
    public SpawnMenu spawnMenu;

    [Header("Debugger")]
    public bool isPlaying;
    public bool gameComplete, newGame;//, isPlaying;

   
    public void Update()
    {
        //DEBUGGING
        /*if (gameComplete)
        {
            isPlaying = false;
            GameComplete();
        }
        if (newGame)
        {
            spawnMenu.ResetMenu();
            isPlaying = true;
            NewGame();
            newGame = false;
        }*/
        //TIMER
        if (isPlaying) //if game is active
        {
            //START TIMER
            score += Time.deltaTime;
            var minutes = score / 60;
            var seconds = score % 60;
            var fraction = (score * 100) % 100;

            //SHOW TIME
            curTime.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        }
    }

    public void NewGame()
    {
        uiComplete.SetActive(false); //hide game complete
        score = 0;

        //SHOW
        miniGame.SetActive(true);
        isPlaying = true;

        //SET ORDER
        for (int i = 0; i < colors.Count; i++)
        {
            bool accepted = false;
            int randomNumb = Random.Range(0, colors.Count);
            while (!accepted && setOrder.Count != 0)
            {
                for (int j = 0; j < setOrder.Count; j++)
                {
                    if (setOrder[j] == randomNumb)
                    {
                        randomNumb = Random.Range(0, colors.Count);
                        break;
                    }
                    else if (setOrder[j] != randomNumb && j == setOrder.Count - 1) accepted = true;
                }               
            }
            setOrder.Add(randomNumb);
        }

        //SET TO CURRENT STATS
        curColor = setOrder[curIndex];
        SpawnBall();
        hoopColor.SetColor("_EmissionColor", colors[curColor].color);

        //START MENU
        spawnMenu.ResetMenu();
    }  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            //CORRECT BALL
            if (other.gameObject.GetComponent<MeshRenderer>().materials[0].name == colors[curColor].name)
            {
                //SHOOT FEEDBACK
                aus.PlayOneShot(shotMade);
                Destroy(other.gameObject);
                GameObject go2 = Instantiate(explosion, other.transform.position, Quaternion.identity);
                var myParticle = go2.GetComponent<ParticleSystem>();
                var main = myParticle.main;
                main.startColor = colors[curColor].color;

                //NEXT COLOR
                if (curIndex < colors.Count - 1) NextColor();
                else GameComplete();
            }
            else //WRONG BALL
            {
                //SHOOT FEEDBACK
                aus.PlayOneShot(errorSound);
                Destroy(other.gameObject);
                GameObject go2 = Instantiate(explosion, other.transform.position, Quaternion.identity);
                var myParticle = go2.GetComponent<ParticleSystem>();
                var main = myParticle.main;
                main.startColor = curMaterial.color;

                //DISPLAY ERROR
                StartCoroutine(ErrorWait());

                //SPAWN BALL
                SpawnBall();
            }
        }
    }
    public void NextColor()
    {
        curIndex++;
        curColor = setOrder[curIndex];
        SpawnBall();
        hoopColor.SetColor("_EmissionColor", colors[curColor].color);
    }
    public void SpawnBall()
    {
        aus.PlayOneShot(respawnBall);

        //SPAWN EXPLOSTION
        GameObject explodeObj = Instantiate(explosion, spawnPt.transform.position, Quaternion.identity);
        var myParticle = explodeObj.GetComponent<ParticleSystem>();
        var main = myParticle.main;
        main.startColor = curMaterial.color;

        //SPAWN BALL
        GameObject go = Instantiate(spawnObj, spawnPt.transform.position, Quaternion.identity);
        go.name = "Ball";
        go.gameObject.GetComponent<MeshRenderer>().materials[0] = curMaterial;
        go.gameObject.GetComponent<MeshRenderer>().materials[0].color = curMaterial.color;
        go.gameObject.GetComponent<MeshRenderer>().materials[0].name = curMaterial.name;
        go.gameObject.GetComponent<TrailRenderer>().startColor = curMaterial.color;
        go.gameObject.GetComponent<TrailRenderer>().endColor = curMaterial.color;

    }

    public void GameComplete()
    {
        //CLEAR
        aus.PlayOneShot(completedSound);
        uiComplete.SetActive(true);
        miniGame.SetActive(false);
        setOrder.Clear();
        Destroy(GameObject.FindGameObjectWithTag("PickUp").gameObject);
        isPlaying = false;
        curIndex = 0;

        //RESULTS
        if (score < highScore) highScore = score; //broke high score
        else if (highScore == 0) highScore = score;
        scoreText.text = string.Format(score.ToString("F2"));
        highScoreText.text = string.Format(highScore.ToString("F2"));
        alcoholMenu.SetActive(true);
        spawnMenu.showItems = true;

        //DEBUGGING
        gameComplete = false;

    }
    IEnumerator ErrorWait() //wrong shot
    {
        errorUI.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        errorUI.SetActive(false);
    }
}
