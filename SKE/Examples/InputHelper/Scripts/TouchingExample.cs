﻿// <copyright>
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
	using SKE.Unity3D.InputHelper;
	using UnityEngine;

	// Simple example on how to use the Touching class.
	public class TouchingExample : MonoBehaviour {
        Vector3 lastCoordinates;

		// Deregister on destruction.
		void OnDestroy() {
			Touching.ObjectTouched -= OnTouch;
		}

		// Print the name of the touched object.
		void OnTouch() {
			print (Touching.Instance.LatestTouch.collider.gameObject.name);
		}

		// Register on start.
		void Start() {
			Touching.ObjectTouched += OnTouch;
		}

		// Check for touches.
		void Update() {
			Touching.Instance.CheckInput (0, true);

            if (Touching.Instance.WorldCoordinates != lastCoordinates) {
                lastCoordinates = Touching.Instance.WorldCoordinates;
                print(lastCoordinates);
            }
		}
	}
}