/**
 * Copyright (c) 2017 The Campfire Union Inc - All Rights Reserved.
 *
 * Licensed under the MIT license. See LICENSE file in the project root for
 * full license information.
 *
 * Email:   info@campfireunion.com
 * Website: https://www.campfireunion.com
 */

using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace VRKeys
{
	public class DemoScene : MonoBehaviour
    {
		public Keyboard keyboard;
        public Color camColor;

        private void OnEnable ()
        {
			GameObject camera = new GameObject ("Main Camera");
			Camera cam = camera.AddComponent<Camera> ();
			cam.nearClipPlane = 0.1f;
			camera.AddComponent<AudioListener> ();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = camColor;

            Canvas canvas = keyboard.canvas.GetComponent<Canvas> ();
			canvas.worldCamera = cam;

			keyboard.Enable ();
			//keyboard.SetPlaceholderMessage ("Please enter your email address");
			keyboard.OnUpdate.AddListener (HandleUpdate);
			keyboard.OnSubmit.AddListener (HandleSubmit);
			//keyboard.OnCancel.AddListener (HandleCancel);
		}

		private void OnDisable () {
			keyboard.OnUpdate.RemoveListener (HandleUpdate);
			keyboard.OnSubmit.RemoveListener (HandleSubmit);
			//keyboard.OnCancel.RemoveListener (HandleCancel);

			keyboard.Disable ();
		}

		/// Press space to show/hide the keyboard.
		private void Update ()
        {
			/*if (Input.GetKeyDown (KeyCode.Space))
            {
				if (keyboard.disabled) keyboard.Enable ();
				else keyboard.Disable ();
			}*/

			if (keyboard.disabled) return;

			/*if (Input.GetKeyDown (KeyCode.Q)) {
				keyboard.SetLayout (KeyboardLayout.Qwerty);
			} else if (Input.GetKeyDown (KeyCode.F)) {
				keyboard.SetLayout (KeyboardLayout.French);
			} else if (Input.GetKeyDown (KeyCode.D)) {
				keyboard.SetLayout (KeyboardLayout.Dvorak);
			}*/
		}

		/// Hide the validation message on update. Connect this to OnUpdate.
		public void HandleUpdate (string text) {keyboard.HideValidationMessage();}

		/// Validate the email and simulate a form submission. Connect this to OnSubmit.
		public void HandleSubmit (string text)
        {
			keyboard.DisableInput ();

			/*if (!ValidateEmail (text)) {
				keyboard.ShowValidationMessage ("Please enter a valid email address");
				keyboard.EnableInput ();
				return;
			}*/
            StartCoroutine (SubmitEmail (text));
        }

	//	public void HandleCancel () { Debug.Log ("Cancelled keyboard input!");}

		/// Pretend to submit the email before resetting.
		private IEnumerator SubmitEmail (string email)
        {
            //keyboard.ShowInfoMessage ("Sending lots of spam, please wait... ;)");

            //yield return new WaitForSeconds (2f);

            //keyboard.ShowSuccessMessage ("Lots of spam sent to " + email);
            keyboard.ShowSuccessMessage("Info: " + email + " received");
           // yield return new WaitForSeconds (2f);
            yield return new WaitForSeconds(.5f);
            keyboard.HideSuccessMessage ();
			keyboard.SetText ("");
			keyboard.EnableInput ();
		}

		/*private bool ValidateEmail (string text) {
			if (!emailValidator.IsMatch (text)) {
				return false;
			}
			return true;
		}*/
	}
}