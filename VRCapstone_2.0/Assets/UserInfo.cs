using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class UserInfo : MonoBehaviour
{
    
    [System.Serializable]
    public class Form
    {
        public string height, weight, biologicalSex;
    }

    public Form form;
 
    public void Awake()
    {
        string path = "Assets/Resources/userInformation.txt";
        StreamReader reader = new StreamReader(path);

        string[] array = reader.ReadLine().Split('|', ':', ' ');

        for (int i = 0; i < array.Length; i++)
        {
            switch (i)
            {
                case 0:
                    form.height = array[i];
                    break;
                case 1:
                    form.weight = array[i];
                    break;
                case 2:
                    form.biologicalSex = array[i];
                    break;
            }
        }
        reader.Close();
    }

    //System.IO.File.WriteAllText(@"Path/foo.bar",string.Empty); //delete data

    /* [System.Serializable]
     public class User
     {
         public string userName, height;
         public int weight;
         public enum BiologicalSex {Female, Male};
         public BiologicalSex biologicalSex;

         public User(string myName, string myHeight, int myWeight, int myBiologicalSex)
         {
             userName = myName;
             height = myHeight;
             weight = myWeight;
            // biologicalSex = myBiologicalSex;
             if (myBiologicalSex == 1) biologicalSex = BiologicalSex.Female;
             else biologicalSex = BiologicalSex.Male;
             Debug.Log(userName + " | " + height + " | " + weight + " | " + biologicalSex.ToString());
         }
     }
     public List<User> user;


     public void CreateNewUser(string myName, string myHeight, int myWeight, int myBiologicalSex)
     {
         user.Add(new User(myName, myHeight, myWeight, myBiologicalSex));
     }*/
}
