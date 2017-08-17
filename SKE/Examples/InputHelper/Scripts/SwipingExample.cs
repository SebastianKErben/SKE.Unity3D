// <copyright>
// MIT License
//
// Copyright (c) 2017 Sebastian K. Erben
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
//	 The above copyright notice and this permission notice shall be included in all
//	 copies or substantial portions of the Software.
//
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//	 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//	 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//	 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//	 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//	 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//	 SOFTWARE.
//</copyright>
//<author>
// Sebastian K. Erben 
// github.com/SebastianKErben
// @SebastianKErben
//</author>

namespace SKE.Examples.InputHelper {
    using System.Collections;

    using SKE.Unity3D.InputHelper;
    using UnityEngine;

    // Simple swipe test using mouse instead of touch.
    public class SwipingExample : MonoBehaviour {
        [SerializeField]
        UnityEngine.UI.Text exampleText;

        // Fields for helping with the coroutine.
        float clearingCountdown;
        bool waitingForClear;

		// Coroutine to clear the text after
		// a certain amount of time.
		IEnumerator ClearText(float countdown) {
			clearingCountdown = countdown;

			if (waitingForClear)
				yield break;

			waitingForClear = true;

			while (clearingCountdown > 0f) {
				yield return new WaitForSeconds(0.1f);
				clearingCountdown -= 0.1f;
			}

			exampleText.text = string.Empty;
			waitingForClear = false;
		}

		void OnDestroy() {
			// Deregister on destruction.
			Swiping.SwipingDown -= SwipeDown;
			Swiping.SwipingUp -= SwipeUp;
			Swiping.SwipingRight -= SwipeRight;
			Swiping.SwipingLeft -= SwipeLeft;
		}

    	void Start () {
            // Init waiting for clear with false,
            // also clear the text.
            waitingForClear = false;
            exampleText.text = string.Empty;

            // Register methods to the swipe events.
            Swiping.SwipingDown += SwipeDown;
            Swiping.SwipingUp += SwipeUp;
            Swiping.SwipingRight += SwipeRight;
            Swiping.SwipingLeft += SwipeLeft;
    	}

		// Swipe down will add swipe down to the text.
		void SwipeDown() {
			exampleText.text += "SwipeDown -- ";
			StartCoroutine(ClearText(1.5f));
		}

		// Swipe left will add swipe left to the text.
		void SwipeLeft() {
			exampleText.text += "SwipeLeft -- ";
			StartCoroutine(ClearText(1.5f));
		}

		// Swipe right will add swipe right to the text.
		void SwipeRight() {
			exampleText.text += "SwipeRight -- ";
			StartCoroutine(ClearText(1.5f));
		}

		// Swipe up will add swipe up to the text.
		void SwipeUp() {
			exampleText.text += "SwipeUp -- ";
			StartCoroutine(ClearText(1.5f));
		}

        void Update() {
            // Check for swipe input.
            Swiping.Instance.CheckInput(0, true);
        }        
    }
}