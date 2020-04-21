using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateKeyboard : MonoBehaviour
{
    int counter = 0;
    public Transform parentObj;
    public string[] numbers;
    public string[] alphabet;

    public int maxSize, temp;
    private VRKeys.Keyboard kyb;

    private void Start()
    {
        kyb = this.GetComponent<VRKeys.Keyboard>();
    }
    void Update()
    {
        /*foreach (Transform eachChild in transform)
        {
            if (eachChild.name == "NameWhatYouNeed")
            {
                Debug.Log("Child found. Mame: " + eachChild.name);
            }
        }*/
        counter = kyb.counter;
        if (kyb.created) maxSize = parentObj.childCount;
        //Debug.Log(this.transform.name + " has " + this.transform.childCount + " children");

        if (counter == 0 || counter == 1) // input numbers
        {
            //if (!kyb.created) kyb.SetupKeys();
            if (kyb.created && temp <= maxSize)
            {
                foreach (Transform child in parentObj)
                {
                    temp++;
                    for (int j = 0; j < numbers.Length; j++)
                    {
                        if (child.name.Contains(numbers[j]))
                        {
                            //Debug.Log("number: " + child.name);
                            break;
                        }
                      //  else child.transform.gameObject.SetActive(true);

                    }
                }
            }
        }
        else
        {
           // if (!kyb.created) kyb.SetupKeys();

            if (kyb.created && temp <= maxSize)
            {
               
                foreach (Transform child in parentObj)
                {
                    temp++;
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (child.name.Contains(alphabet[j]))
                        {
                           // Debug.Log("letter: " + child.name);
                            break;
                        }
                       // else child.transform.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
