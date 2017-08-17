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

namespace SKE.Examples.Analyze {
	using System.Collections;

	using SKE.Unity3D.Analyze;
	using UnityEngine;

	// Simple logging test.
	public class LoggingExample : MonoBehaviour {
		bool logging;

		IEnumerator LogForSeconds(int seconds) {
			// Make sure we have only one thread logging.
			if (logging)
				yield break;

			// Log that we are about to log.
			Logging.Instance.Log ("Start");
			logging = true;

			// One entry per second.
			while (seconds > 0) {
				Logging.Instance.Log ("Another entry to the log");
				yield return new WaitForSeconds (1f);
				--seconds;
			}

			// We're done.
			logging = false;
			Logging.Instance.Log ("End");
		}

		void Start () {
			// Activate log output to Unitys console log.
			Logging.Instance.LogToDebug = true;

			// Print some log messages.
			StartCoroutine (LogForSeconds (5));
		}
	}
}