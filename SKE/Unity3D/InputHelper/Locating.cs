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
    using System.Collections;

    using SKE.Unity3D.Singleton;
    using UnityEngine;

    /// <summary>
    /// Locating singleton.
    /// </summary>
    public class Locating : SingletonEternal<Locating> {
		LocationInfo latestLocation;
		bool locationRunning;
		LocationService locationService;
        bool locationSuccess;        

		/// <summary>
		/// Location finished event handler.
		/// </summary>
        public delegate void LocationFinishedEventHandler();

        /// <summary>
        /// Occurs when location finished.
        /// </summary>
        public static event LocationFinishedEventHandler LocationFinished;

        /// <summary>
        /// Gets the latest location info.
        /// </summary>
        /// <value>The latest info.</value>
        public LocationInfo LatestInfo {
            get {
                return latestLocation;
            }
        }

        /// <summary>
        /// Gets the latest location.
        /// </summary>
        /// <value>The latest location.</value>
        public Vector3 LatestLocation {
            get {
                if (!locationSuccess)
                    return Vector3.zero;

                return new Vector3(
                    latestLocation.latitude, 
                    latestLocation.longitude, 
                    latestLocation.altitude);
            }
        }

        /// <summary>
        /// Gets a value indicating whether
        /// <see cref="SKE.Unity3D.InputHelper.Locating"/> location is possible.
        /// </summary>
        /// <value><c>true</c> if location possible; otherwise, <c>false</c>.</value>
        public bool LocationPossible {
            get {
                return locationService.isEnabledByUser && 
                    locationService.status.Equals(LocationServiceStatus.Running);
            }
        }

        /// <summary>
        /// Checks the input for location data.
        /// </summary>
        public void CheckInput() {
			if (locationService == null)
				locationService = Input.location;
			
            if (!locationService.isEnabledByUser || locationRunning)
                return;

            StartCoroutine(GetLocation());
        }

		/// <summary>
		/// Will be called once the Singleton awakes the
		/// first time.
		/// </summary>
		public override void SingletonAwake () {
			locationRunning = false;
			locationSuccess = false;
		}

        IEnumerator GetLocation() {
            locationRunning = true;

            while (locationService == null)
                yield return new WaitForSeconds(1f);

            locationService.Start();

            while(!locationService.status.Equals(LocationServiceStatus.Running)) {
                if (!locationService.status.Equals(LocationServiceStatus.Initializing)) {
                    locationRunning = false;
                    locationSuccess = false;
                    yield break;
                }

                yield return new WaitForSeconds(1f);
            }

            locationSuccess = true;
            latestLocation = locationService.lastData;

            locationService.Stop();

            while (!locationService.status.Equals(LocationServiceStatus.Stopped))
                yield return new WaitForSeconds(1f);

            locationRunning = false;
            LocationFinished();
        }
    }
}