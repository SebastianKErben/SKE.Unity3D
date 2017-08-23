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

namespace SKE.Unity3D.InputHelper {
    using System;

    using SKE.Unity3D.Singleton;
    using UnityEngine;

    /// <summary>
    /// Swiping singleton.
    /// </summary>
    public class Swiping : SingletonEternal<Swiping> {
        public readonly SwipingOperator swipingOperator = new SwipingOperator();

        Camera currentCamera;
        float swipeTreshold;

		/// <summary>
		/// Swiping down event handler.
		/// </summary>
		public delegate void SwipingDownEventHandler();
		/// <summary>
		/// Swiping left event handler.
		/// </summary>
		public delegate void SwipingLeftEventHandler();
		/// <summary>
		/// Swiping right event handler.
		/// </summary>
		public delegate void SwipingRightEventHandler();
		/// <summary>
		/// Swiping up event handler.
		/// </summary>
		public delegate void SwipingUpEventHandler();

		/// <summary>
		/// Occurs when swiping down.
		/// </summary>
		public static event SwipingDownEventHandler SwipingDown;
		/// <summary>
		/// Occurs when swiping left.
		/// </summary>
		public static event SwipingLeftEventHandler SwipingLeft;
		/// <summary>
		/// Occurs when swiping right.
		/// </summary>
		public static event SwipingRightEventHandler SwipingRight;
		/// <summary>
		/// Occurs when swiping up.
		/// </summary>
		public static event SwipingUpEventHandler SwipingUp;

		/// <summary>
		/// Gets or sets the current camera.
		/// </summary>
		/// <value>The raycast camera.</value>
		public Camera CurrentCamera {
			set {
				currentCamera = value;
			}

			get {
                return currentCamera ?? Camera.main;
			}
		}

        /// <summary>
        /// Gets or sets the swipe threshold.
        /// </summary>
        /// <value>The swipe threshold.</value>
        public float SwipeThreshold {
            set {
                swipeTreshold = value >= 0f ? value : -value;
            }

            get {
                return swipeTreshold;
            }
        }

		/// <summary>
		/// Checks the input for swipes.
		/// </summary>
		/// <param name="index">Index of the input.</param>
		/// <param name="mouse">If set to <c>true</c> check mouse
		/// instead of touc input.</param>
		public void CheckInput(int index=0, bool mouse=false) {
			Vector3 inputPosition;

			if (mouse) {
				inputPosition = Input.mousePosition;

				if (Input.GetMouseButtonDown(index))
					RegisterSwipeStart(ref inputPosition);
				else if (Input.GetMouseButtonUp(index))
					RegisterSwipeEnd(ref inputPosition);
			}
			else {
				var touch = Input.GetTouch(index);
				inputPosition = touch.position;

				switch (touch.phase) {
				case TouchPhase.Began:
					RegisterSwipeStart(ref inputPosition);
					break;

				case TouchPhase.Canceled:
					RegisterSwipeEnd(ref inputPosition);
					break;

				case TouchPhase.Ended:
					RegisterSwipeEnd(ref inputPosition);
					break;
				}
			}


		}

		void RegisterSwipeEnd(ref Vector3 inputPosition) {
            var swipeVector = swipingOperator.RegisterEnd(CurrentCamera.ScreenToViewportPoint(inputPosition));

			if (swipeVector.x > SwipeThreshold && SwipingRight != null)
				SwipingRight();
			else if (swipeVector.x < -SwipeThreshold && SwipingLeft != null)
				SwipingLeft();

			if (swipeVector.y > SwipeThreshold && SwipingUp != null)
				SwipingUp();
			else if (swipeVector.y < -SwipeThreshold && SwipingDown != null)
				SwipingDown();
		}

        void RegisterSwipeStart(ref Vector3 inputPosition) {
            swipingOperator.RegisterStart(CurrentCamera.ScreenToViewportPoint(inputPosition));
        }        
    }

    /// <summary>
    /// Swiping operator class.
    /// </summary>
    [Serializable]
    public class SwipingOperator {
        Vector2 latestSwipe;
		Vector2 swipeEnd;
		bool swipeFinished;
        Vector2 swipeStart;        

        /// <summary>
        /// Gets the latest swipe.
        /// </summary>
        /// <value>The latest swipe.</value>
        public Vector2 LatestSwipe {
            get {
                return latestSwipe.normalized;
            }
        }

        /// <summary>
        /// Registers the end.
        /// </summary>
        /// <returns>The end.</returns>
        /// <param name="destination">Destination.</param>
        public Vector2 RegisterEnd(Vector3 destination) {
            if (!swipeFinished)
                swipeEnd = new Vector2(destination.x, destination.y);

            swipeFinished = true;
            latestSwipe = swipeEnd - swipeStart;

            return LatestSwipe;
        }

        /// <summary>
        /// Registers the start.
        /// </summary>
        /// <param name="origin">Origin.</param>
        public void RegisterStart(Vector3 origin) {
            swipeFinished = false;
            swipeStart = new Vector2(origin.x, origin.y);
        }
    }
}