using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnMenu : MonoBehaviour
{
    [Header("Radial Menu")]
    public Transform spawnPt;
    public float moveSpeed;
    public List<GameObject> spawnObjs;

    private bool addIndex = true;
    private int anglePerObject;
    [HideInInspector] public float tempTime = 0;
    [HideInInspector] public int curAngle = 0, curIndex, temp;
    [HideInInspector] public bool moving = true, calledOnce;
    [HideInInspector] public List<GameObject> newPositions;
    [HideInInspector] public List<Vector3> objPositions;

    [Header("Alcohol Stats")]
    public Text drinkTitle;
    public InfoStats[] infoStats;
    [System.Serializable]
    public class InfoStats
    {
        public Text infoName;
        public Text description;
    }

    [Header("Scale Stats")]
    public float speed = 2.0f;
    public Vector3 unselectedSize, selectedSize;

    [Header("Selection Stats")]
    public GameObject alcoholMenu;
    public GameObject playButton, spawnCup;
    public Material selectedItem, unselectedItem, hoverItem;
    public bool showItems, spawned, canMove;
    public RotateDetection leftHand;

    void Start()
    {
        anglePerObject = 360 / spawnObjs.Count;
    }
    private void LateUpdate()
    {
        #region SPAWN MENU
        if (curAngle < 360) //spawn menu
        {
            this.transform.RotateAround(this.transform.position, transform.up, Time.timeScale);
            tempTime += Time.timeScale; //* speed;

            if (Mathf.CeilToInt(tempTime) >= curAngle)
            {
                newPositions.Add(Instantiate(spawnObjs[curIndex], spawnPt.transform.position, this.transform.rotation)); //instantiate item
                objPositions.Add(newPositions[curIndex].transform.position); //add instantiated item to new Positions
                newPositions[curIndex].SetActive(false); //deactivate item
                curAngle += anglePerObject;
                curIndex++;
            }
            spawned = true;
        }
        #endregion
        else //use menu
        {
            #region TRAVERSE MENU
            if (curIndex >= spawnObjs.Count) curIndex = 0;
            if (moving && newPositions.Count > 0 && canMove) //rotate menu
            {
                //  if (curIndex >= spawnObjs.Count) curIndex = 0;
                if (addIndex)
                {
                    //curIndex++;
                    curIndex--;
                    temp = curIndex;
                    addIndex = false;
                }
                if (curIndex < 0) curIndex = spawnObjs.Count - 1;

                /*for (int i = 0; i < newPositions.Count; i++) //move objects left
                {
                    if (temp > newPositions.Count - 1)
                    {
                        temp = 0;
                        StartCoroutine(MoveMenu(newPositions[i].transform, objPositions[temp], i));
                        temp++;
                    }
                    else
                    {
                        StartCoroutine(MoveMenu(newPositions[i].transform, objPositions[temp], i));
                        temp++;
                    }
                }*/
                for (int i = 0; i < newPositions.Count; i++) //move objects right
                {
                    if (temp > 1) temp -= newPositions.Count;
                    if (temp < 0) temp = newPositions.Count - 1;

                    /////////// MISC ///////////////////
                    if (objPositions[temp] == objPositions[0]) //selected item
                    {
                        drinkTitle.text = spawnObjs[i].GetComponent<Alcohol_Stats>().title; //set obj title
                        for (int j = 0; j < infoStats.Length; j++) //get info
                        {
                            infoStats[j].infoName.text = spawnObjs[i].GetComponent<Alcohol_Stats>().stats[j].infoName; //set info name
                            infoStats[j].description.text = spawnObjs[i].GetComponent<Alcohol_Stats>().stats[j].info; //set info
                        }
                        newPositions[i].GetComponent<Transform>().transform.localScale = Vector3.Lerp(newPositions[i].GetComponent<Transform>().transform.localScale, selectedSize, speed * Time.deltaTime);
                        newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = selectedItem;
                        newPositions[i].GetComponent<Alcohol_Stats>().isCenter = true;
                        //BOX COLLIDERS
                       // newPositions[i].GetComponent<BoxCollider>().enabled = true;
                       // Component[] colliders;
                       // colliders = GetComponents(typeof(BoxCollider));
                      //  foreach (BoxCollider myBox in colliders) myBox.enabled = true;
                        for (int j = 0; j < newPositions[i].GetComponents<BoxCollider>().Length; j++)
                            newPositions[i].GetComponents<BoxCollider>()[j].enabled = true;
                    }
                    else //unselected items
                    {
                        // newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<Transform>().transform.Rotate(-90, 0, 0);
                        newPositions[i].GetComponent<Transform>().transform.localScale = Vector3.Lerp(newPositions[i].GetComponent<Transform>().transform.localScale, unselectedSize, speed * Time.deltaTime);

                        //BOX COLLIDERS
                        for (int j = 0; j < newPositions[i].GetComponents<BoxCollider>().Length; j++) newPositions[i].GetComponents<BoxCollider>()[j].enabled = false;

                        newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = unselectedItem;
                        newPositions[i].GetComponent<Alcohol_Stats>().hover = false;
                        newPositions[i].GetComponent<Alcohol_Stats>().isCenter = false;
                    }
                    /////////////////////////////////////
                    if (temp < 0)
                    {
                        temp = 0;
                        StartCoroutine(MoveMenu(newPositions[i].transform, objPositions[temp], i));
                        temp++;
                    }
                    else
                    {
                        StartCoroutine(MoveMenu(newPositions[i].transform, objPositions[temp], i));
                        temp++;
                    }
                }
            }
            #endregion

            if (spawned)
            {
                for (int i = 0; i < objPositions.Count; i++) //hand hover detection & show items
                {
                    if (showItems) newPositions[i].SetActive(true); //show items

                    #region GRAB ITEM
                    if (newPositions[i].GetComponent<Alcohol_Stats>().grabbed) //on grab
                    {
                        //CLEAN PREVIOUS
                        canMove = false;
                        if (GameObject.Find("UserBottle") != null)
                        {
                            Destroy(GameObject.Find("UserBottle")); 
                            Destroy(GameObject.Find("UserCup")); 
                        }

                        //SPAWN
                        newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = hoverItem;
                        newPositions[i].name = "UserBottle";
                        GameObject go = Instantiate(newPositions[i].GetComponent<Alcohol_Stats>().cupObj, spawnCup.transform.position, Quaternion.identity); // spawn cup
                        go.name = "UserCup";

                        //RESET 
                        leftHand.inside = false;
                        for (int j = 0; j < newPositions.Count; j++) if (i != j) Destroy(newPositions[j].gameObject); //destroy bottles
                        newPositions.Clear();
                        showItems = false;
                        alcoholMenu.SetActive(false);
                        playButton.SetActive(true);                  
                        spawned = false;                  
                    }
                    #endregion
                    #region Hover ITEM
                    if (newPositions[i].GetComponent<Alcohol_Stats>().isCenter) // is selected item
                    {
                        if (newPositions[i].GetComponent<Alcohol_Stats>().hover) // on hover
                        {
                            newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = hoverItem;
                            newPositions[i].GetComponent<Alcohol_Stats>().liquidObj.SetActive(true);
                        }
                        else // leave hover
                        {
                            newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = selectedItem;
                            newPositions[i].GetComponent<Alcohol_Stats>().liquidObj.SetActive(false);
                        }
                    }
                    else //is unselected 
                    {
                        newPositions[i].GetComponent<Alcohol_Stats>().bottleObj.GetComponent<MeshRenderer>().material = unselectedItem;
                        newPositions[i].GetComponent<Alcohol_Stats>().liquidObj.SetActive(false);
                    }
                    #endregion
                }
            }
        }
    }

    private IEnumerator MoveMenu(Transform fromPosition, Vector3 toPosition, int myNumb) //Move nodes
    {

        float counter = 0;
        Vector3 startPos = fromPosition.position;
        moving = true;

        while (counter < moveSpeed)
        {
            counter += Time.deltaTime;
            fromPosition.position = Vector3.Lerp(startPos, toPosition, counter / moveSpeed);
            yield return null;
        }
        if (myNumb == newPositions.Count - 1) //has finished
        {
            moving = false;
            addIndex = true;
        }        
    }
    public void ResetMenu() //spawn menu again
    {
        canMove = true;
        curAngle = 0;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        if (!calledOnce)
        {
            for (int j = 0; j < newPositions.Count; j++) Destroy(newPositions[j].gameObject);
            calledOnce = true;
        }
        newPositions.Clear();
        objPositions.Clear();
        curAngle = 0;
        curIndex = 0;
        tempTime = 0;
        moving = true;
    }
}