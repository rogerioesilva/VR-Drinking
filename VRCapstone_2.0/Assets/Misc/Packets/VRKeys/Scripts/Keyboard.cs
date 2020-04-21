using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


namespace VRKeys
{
    [System.Serializable]
    public class User
    {
        public string height;
        public string weight;
        public enum BiologicalSex { Female, Male };
        public BiologicalSex biologicalSex;
    }
    public class Keyboard : MonoBehaviour
    {
        public List<User> user;

        /// ///////////////////////////
		public Vector3 positionRelativeToUser = new Vector3 (0f, 1.35f, 2f);
		public KeyboardLayout keyboardLayout = KeyboardLayout.Qwerty;

		[Space (15)]
		public TextMeshProUGUI placeholder;
		public string placeholderMessage = "Tap the keys to begin typing";
		public TextMeshProUGUI displayText;
		public GameObject validationNotice;
		public TextMeshProUGUI validationMessage;
		public GameObject infoNotice;
		public TextMeshProUGUI infoMessage;
		public GameObject successNotice;
		public TextMeshProUGUI successMessage;

		[Space (15)]
		public Color displayTextColor = Color.black;
		public Color caretColor = Color.gray;

		[Space (15)]
		public GameObject keyPrefab;
		public Transform keysParent;
		public float keyWidth = 0.16f, keyHeight = 0.16f;

		[Space (15)]
		public string text = "";

		[Space (15)]
        public ShiftKey shiftKey;
        public GameObject canvas, leftMallet, rightMallet, keyboardWrapper;
		public Key[] extraKeys;

		[Space (15)]
		public bool leftPressing = false;
		public bool rightPressing = false, initialized = false, disabled = true;

		[Serializable]
		public class KeyboardUpdateEvent : UnityEvent<string> { }

		[Serializable]
		public class KeyboardSubmitEvent : UnityEvent<string> { }

		[Space (15)]
		public KeyboardUpdateEvent OnUpdate = new KeyboardUpdateEvent ();
		public KeyboardSubmitEvent OnSubmit = new KeyboardSubmitEvent ();
		public UnityEvent OnCancel = new UnityEvent ();
		private GameObject playerSpace, leftHand, rightHand;
		private LetterKey[] keys;
		private bool shifted = false;
		private Layout layout;

        /// /////////////////////////////// MY VARIABLES
        public int counter, index;
        public string[] questions;
        public TextMeshProUGUI infoText;
        public Material matDisable;
        public bool created;
        /// ////////////////////////
        public void Awake() { infoText.text = questions[counter]; }

        private IEnumerator Start ()
        {

            XRDevice.SetTrackingSpaceType (TrackingSpaceType.RoomScale);

			playerSpace = new GameObject ("Play Space");
			leftHand = new GameObject ("Left Hand");
			rightHand = new GameObject ("Right Hand");

			yield return StartCoroutine (DoSetLanguage (keyboardLayout));

			validationNotice.SetActive (false);
			infoNotice.SetActive (false);
			successNotice.SetActive (false);
			UpdateDisplayText ();
			PlaceholderVisibility ();
			initialized = true;
		}

		private void Update ()
        {
			leftHand.transform.localPosition = InputTracking.GetLocalPosition (XRNode.LeftHand);
			leftHand.transform.localRotation = InputTracking.GetLocalRotation (XRNode.LeftHand);
			rightHand.transform.localPosition = InputTracking.GetLocalPosition (XRNode.RightHand);
			rightHand.transform.localRotation = InputTracking.GetLocalRotation (XRNode.RightHand);
		}

        #region MALLETS
        private void PositionAndAttachMallets ()
        {
			transform.SetParent (playerSpace.transform, false);
			transform.localPosition = positionRelativeToUser;

			leftMallet.transform.SetParent (leftHand.transform);
			leftMallet.transform.localPosition = Vector3.zero;
			leftMallet.transform.localRotation = Quaternion.Euler (90f, 0f, 0f);
			leftMallet.SetActive (true);

			rightMallet.transform.SetParent (rightHand.transform);
			rightMallet.transform.localPosition = Vector3.zero;
			rightMallet.transform.localRotation = Quaternion.Euler (90f, 0f, 0f);
			rightMallet.SetActive (true);
		}
		private void DetachMallets ()
        {
			if (leftMallet != null) leftMallet.SetActive (false);
			if (rightMallet != null) rightMallet.SetActive (false);
		}
		/// Make sure mallets don't stay attached if VRKeys is disabled without
		private void OnDisable () {Disable ();}
        #endregion

        #region KEYBOARD
        public void Enable ()
        {
			if (!initialized)
            {
				StartCoroutine (EnableWhenInitialized ());
				return;
			}

			disabled = false;
			if (canvas != null) canvas.SetActive (true);
			if (keysParent != null) keysParent.gameObject.SetActive (true);
			EnableInput ();
			PositionAndAttachMallets ();
		}
		private IEnumerator EnableWhenInitialized ()
        {
			yield return new WaitUntil (() => initialized);
			Enable ();
		}
		public void Disable ()
        {
			disabled = true;
			if (canvas != null) canvas.SetActive (false);
			if (keysParent != null) keysParent.gameObject.SetActive (false);
			DetachMallets ();
		}

		/// Set the text value all at once.
		public void SetText (string txt)
        {
			text = txt;
			UpdateDisplayText ();
			PlaceholderVisibility ();
			OnUpdate.Invoke (text);
		}

		/// Add a character to the input text.
		public void AddCharacter (string character)
        {
            if (counter == 2) text = character;
            else text += character;
			UpdateDisplayText ();
			PlaceholderVisibility ();
			OnUpdate.Invoke (text);
		//	if (shifted && character != "" && character != " ") StartCoroutine (DelayToggleShift ());
		}
		/*public bool ToggleShift ()
        {
			if (keys == null) return false;
			shifted = !shifted;
			foreach (LetterKey key in keys) key.shifted = shifted;
			shiftKey.Toggle (shifted);
			return shifted;
		}

		private IEnumerator DelayToggleShift ()
        {
			yield return new WaitForSeconds (0.1f);
			ToggleShift ();
		}*/
		public void DisableInput ()
        {
			leftPressing = false;
			rightPressing = false;

			if (keys != null)
            {
				foreach (LetterKey key in keys)
                {
					if (key != null) key.Disable ();
				}
			}
			foreach (Key key in extraKeys) key.Disable ();
		}
		public void EnableInput ()
        {
			leftPressing = false;
			rightPressing = false;

			PositionAndAttachMallets ();

			if (keys != null)
            {
				foreach (LetterKey key in keys)
                {
					if (key != null) key.Enable ();
				}
			}

			foreach (Key key in extraKeys) key.Enable ();
		}
        #endregion

        /// Backspace one character.
        public void Backspace ()
        {
			if (text.Length > 0) text = text.Substring (0, text.Length - 1);
			UpdateDisplayText ();
			PlaceholderVisibility ();
			OnUpdate.Invoke (text);
		}

		/// Submit and close the keyboard.
        /// ///////////////////////////////////////////////////////////////////////////////////////
		public void Submit ()
        {
            switch (counter)
            {
                case 0:
                    user.Add(new User());
                    user[index].height = text;
                    counter++;
                    infoText.text = questions[counter];
                    created = false;
                    StartCoroutine(SetupKeys());
                    break;
                case 1:
                    //user[index].weight = int.Parse(text);
                    user[index].weight = text;
                    counter++;
                    infoText.text = questions[counter];
                    created = false;
                    StartCoroutine(SetupKeys());
                    break;
                case 2:
                    if (text.Contains("f")) user[index].biologicalSex = User.BiologicalSex.Female;
                    else user[index].biologicalSex = User.BiologicalSex.Male;
                    Debug.Log(user[index].height + " | " + user[index].weight + " | " + user[index].biologicalSex.ToString());
                    created = false;

                    //write file
                    string path = "Assets/Resources/userInformation.txt";
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine(user[index].height + " | " + user[index].weight + " | " + user[index].biologicalSex.ToString());
                    writer.Close();
                    index++;
                    //SceneManager.LoadScene("Training", LoadSceneMode.Additive);
                    SceneManager.LoadScene("Training");
                    break;

            }
            OnSubmit.Invoke (text);
		}
       
        /// Cancel input and close the keyboard.
      /*  public void Cancel ()
        {
			OnCancel.Invoke ();
			Disable ();
		}*/

		/// Set the language of the keyboard.
		public void SetLayout (KeyboardLayout layout) { StartCoroutine (DoSetLanguage (layout)); }
		private IEnumerator DoSetLanguage (KeyboardLayout lang)
        {
			keyboardLayout = lang;
			layout = LayoutList.GetLayout (keyboardLayout);
			placeholderMessage = layout.placeholderMessage;
			yield return StartCoroutine (SetupKeys ());
			// Update extra keys
			foreach (Key key in extraKeys) key.UpdateLayout (layout);
		}

        /// Set a custom placeholder message.
        public void SetPlaceholderMessage(string msg) { StartCoroutine(DoSetPlaceholderMessage(msg)); }

		private IEnumerator DoSetPlaceholderMessage (string msg)
        {
			if (!initialized) yield return new WaitUntil (() => initialized);
			placeholder.text = placeholderMessage = msg;
			yield break;
		}

		/// Show the specified validation notice.
		public void ShowValidationMessage (string message)
        {
			validationMessage.text = message;
			validationNotice.SetActive (true);
			infoNotice.SetActive (false);
			successNotice.SetActive (false);
		}

		/// Show the specified input notice.
		public void ShowInfoMessage (string message)
        {
			infoMessage.text = message;
			validationNotice.SetActive (false);
			infoNotice.SetActive (true);
			successNotice.SetActive (false);
		}

		/// Show the specified success notice.
		public void ShowSuccessMessage (string message)
        {
			successMessage.text = message;
			validationNotice.SetActive (false);
			infoNotice.SetActive (false);
			successNotice.SetActive (true);
		}

		/// Hide the validation notice.
		public void HideValidationMessage () { validationNotice.SetActive (false); }

		/// Hide the info notice.
		public void HideInfoMessage () { infoNotice.SetActive (false); }

		/// Hide the success notice.
		public void HideSuccessMessage () {	successNotice.SetActive (false);}

        /// Setup the keys.
        /// /////////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerator SetupKeys()
        {
            bool activeState = canvas.activeSelf;
            canvas.SetActive(false);
            keysParent.gameObject.SetActive(false);

            // Remove previous keys
            if (keys != null) foreach (Key key in keys) if (key != null) Destroy(key.gameObject);

            keys = new LetterKey[layout.TotalKeys()];
            int keyCount = 0;

            // Numbers row
           // int startNumbers;
            //if (counter == 0) startNumbers = 0;
            //else startNumbers = 1;

            for (int i = 1; i < layout.row1Keys.Length-2; i++)
            {
                GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
                //  if(counter == 0) obj.transform.localPosition += (Vector3.right * ((keyWidth * i) + .14f - layout.row1Offset));
                // else obj.transform.localPosition += (Vector3.right * ((keyWidth * i) + .13f - layout.row1Offset));
                obj.transform.localPosition += (Vector3.right * ((keyWidth * i) +.1f - layout.row1Offset));

                obj.transform.localPosition += (Vector3.back * keyHeight * 2);

                LetterKey key = obj.GetComponent<LetterKey>();
                key.character = layout.row1Keys[i];
               // key.shiftedChar = layout.row1Shift[i];
                key.shifted = false;
                key.Init(obj.transform.localPosition);

                obj.name = "Key: " + layout.row1Keys[i];
                //obj.SetActive(true);

                if (counter == 0 || counter == 1) // needs letters
                {
                    obj.SetActive(true);
                   
                }
                else
                {
                    obj.GetComponent<MeshRenderer>().material = matDisable;
                    obj.GetComponent<BoxCollider>().enabled = false;
                    obj.SetActive(false);
                    obj.GetComponent<LetterKey>().enabled = false;
                  //  obj.SetActive(true);

                }

                keys[keyCount] = key;
                keyCount++;

                yield return null;
            }

            // QWERTY row
            for (int i = 0; i < layout.row2Keys.Length; i++)
            {
                GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
                obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row2Offset));
                obj.transform.localPosition += (Vector3.back * keyHeight * 1);

                LetterKey key = obj.GetComponent<LetterKey>();
                key.character = layout.row2Keys[i];
              //  key.shiftedChar = layout.row2Shift[i];
                key.shifted = false;
                key.Init(obj.transform.localPosition);

                obj.name = "Key: " + layout.row2Keys[i];
                // obj.SetActive(true);

                if (counter == 0 || counter == 1) // needs numbers
                {
                    obj.GetComponent<MeshRenderer>().material = matDisable;
                    obj.GetComponent<BoxCollider>().enabled = false;
                    obj.SetActive(false);
                    obj.GetComponent<LetterKey>().enabled = false;
                 //   obj.SetActive(true);

                }

                else
                {
                    // obj.SetActive(true);
                   // if (layout.row2Keys[i] == "m") obj.SetActive(true);
                }

                keys[keyCount] = key;
                keyCount++;

                yield return null;
            }

            // ASDF row
            for (int i = 0; i < layout.row3Keys.Length; i++)
            {
                GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
                obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row3Offset));
                obj.transform.localPosition += (Vector3.back * keyHeight * 2);

                LetterKey key = obj.GetComponent<LetterKey>();
               // key.character = layout.row3Keys[i];
                key.character = layout.row3Shift[i];
                //key.shiftedChar = layout.row3Shift[i];
                key.shifted = false;
                key.Init(obj.transform.localPosition);
                obj.name = "Key: " + layout.row3Keys[i];
                // obj.SetActive(true);

                if (counter == 0 || counter == 1) // needs numbers
                {
                    obj.GetComponent<MeshRenderer>().material = matDisable;
                    obj.GetComponent<BoxCollider>().enabled = false;
                    obj.SetActive(false);
                    obj.GetComponent<LetterKey>().enabled = false;
                  //  obj.SetActive(true);
                }
                else if(layout.row3Keys[i] == "f") obj.SetActive(true);

                keys[keyCount] = key;
                keyCount++;
                yield return null;
            }

            // ZXCV row
            for (int i = 0; i < layout.row4Keys.Length; i++)
            {
                GameObject obj = (GameObject)Instantiate(keyPrefab, keysParent);
                obj.transform.localPosition += (Vector3.right * ((keyWidth * i) - layout.row4Offset));
                obj.transform.localPosition += (Vector3.back * keyHeight * 2); //3

                LetterKey key = obj.GetComponent<LetterKey>();
                // key.character = layout.row4Keys[i];
                key.character = layout.row4Shift[i];
                // key.shiftedChar = layout.row4Shift[i];
                key.shifted = false;
                obj.name = "Key: " + layout.row4Keys[i];
                key.Init(obj.transform.localPosition);
                //obj.SetActive(true);

                if (counter == 0 || counter == 1) // needs numbers
                {
                    obj.GetComponent<MeshRenderer>().material = matDisable;
                    obj.GetComponent<BoxCollider>().enabled = false;
                    obj.SetActive(false);
                    obj.GetComponent<LetterKey>().enabled = false;
                    // obj.SetActive(true);

                }
                else if (layout.row4Keys[i] == "m") obj.SetActive(true);
                keys[keyCount] = key;
                keyCount++;

                yield return null;
            }

            // Reset visibility of canvas and keyboard
            canvas.SetActive(activeState);
            keysParent.gameObject.SetActive(activeState);
            created = true;
        }

		/// Update the display text, including trailing caret.
		private void UpdateDisplayText ()
        {
			string display = (text.Length > 37) ? text.Substring (text.Length - 37) : text;

			displayText.text = string.Format (
				"<#{0}>{1}</color><#{2}>_</color>",
				ColorUtility.ToHtmlStringRGB (displayTextColor),
				display,
				ColorUtility.ToHtmlStringRGB (caretColor)
			);
		}

		/// Show/hide placeholder text.
		private void PlaceholderVisibility ()
        {
			if (text == "")
            {
				placeholder.text = placeholderMessage;
				placeholder.gameObject.SetActive (true);
			} else placeholder.gameObject.SetActive (false);
		}
	}
}