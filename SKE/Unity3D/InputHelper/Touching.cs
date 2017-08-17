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
    using SKE.Unity3D.Singleton;
    using UnityEngine;

    /// <summary>
    /// Touching singleton.
    /// </summary>
    public class Touching : SingletonEternal<Touching> {
        float depthThreshold = Mathf.Infinity;
        Ray inputRay;
        Camera raycastCamera;
        RaycastHit rayHit;

		/// <summary>
		/// Object touched event handler.
		/// </summary>
		public delegate void ObjectTouchedEventHandler();

		/// <summary>
		/// Occurs when object touched.
		/// </summary>
		public static event ObjectTouchedEventHandler ObjectTouched;

		/// <summary>
		/// Gets or sets the depth threshold.
		/// </summary>
		/// <value>The depth threshold.</value>
		public float DepthThreshold {
			set {
				depthThreshold = value < 0f ? 0f : value;
			}
			get {
				return depthThreshold;
			}
		}

        /// <summary>
        /// Gets the latest touch.
        /// </summary>
        /// <value>The latest touch.</value>
        public RaycastHit LatestTouch {
            get { return rayHit; }
        }

        /// <summary>
        /// Gets or sets the raycast camera.
        /// </summary>
        /// <value>The raycast camera.</value>
        public Camera RaycastCamera {
            set {
                raycastCamera = value;
            }
            get {
                return raycastCamera ?? Camera.main;
            }
        }
		
        /// <summary>
        /// Checks the input for touch occurance.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="mouse">If set to <c>true</c> mouse.</param>
        public void CheckInput(int index=0, bool mouse=false) {
            if (mouse && (Input.GetMouseButtonDown(index)))
                HandleTouch(Input.mousePosition);
            else if (Input.touchCount > index && 
                Input.GetTouch(index).phase.Equals(TouchPhase.Began))
                HandleTouch(Input.GetTouch(index).position);
        }

        void HandleTouch(Vector3 inputPosition) {
            inputRay = RaycastCamera.ScreenPointToRay(inputPosition);

            if (!Physics.Raycast(inputRay, out rayHit, depthThreshold))
                return;

            ObjectTouched();
        }
    }
}