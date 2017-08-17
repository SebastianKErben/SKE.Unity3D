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

namespace SKE.Examples.Singleton {
    using System.Collections;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SingletonExampleManager : MonoBehaviour {
        // This is our singleton that will not survive scene switching.
        TestSingleton myTestSingleton;

        TestEternal myEternalSingleton;

        void Awake () {
            // By calling the static property Instance of our TestSingleton
            // class, we create a new one because there doesn't exist one
            // already.
            myTestSingleton = TestSingleton.Instance;

            // Now we're going to test if our singleton actually behaves
            // like a singleton should.
            StartCoroutine(TestTheSingleton());

            // By calling the static property Instance of our TestEternal
            // class, we create a new one only once at the first scene.
            // Even switching scene when this gets called again, the instance
            // should be the same.
            myEternalSingleton = TestEternal.Instance;

            myEternalSingleton.LastLoadedScene = 
                SceneManager.GetActiveScene().name;

            // Test if the eternal singleton is always the same.
            StartCoroutine(TestForever());
        }

        IEnumerator TestForever() {
            // Eternal time should never change, even after scene switches.
            while (true) {
                myEternalSingleton = TestEternal.Instance;
                Debug.Log("Eternal time: " + myEternalSingleton.CreationTime);

                yield return new WaitForSeconds(2f);
            }
        }

        IEnumerator TestTheSingleton() {
            // Give us the original creation time of our test singleton.
            Debug.Log("Creation time: " + myTestSingleton.CreationTime);

            // Now we're going to call Instance of TestSingleton three
            // times. Since it already exists, no new instance should be
            // created and thus the creation time should always stay the
            // same.
            for (var iterator = 0; iterator != 3; ++iterator) {
                yield return new WaitForSeconds(1f);

                myTestSingleton = TestSingleton.Instance;
                Debug.Log("Creation time: " + myTestSingleton.CreationTime);
            }

            Debug.Log("Creation time should have been the same every time.");
        }

        void Update() {
            // Switch between both example scenes on any button press.
            if (Input.anyKeyDown) {
                var sceneToLoad = 
                    myEternalSingleton.LastLoadedScene.Equals("singletonExampleA") ?
                    "singletonExampleB" : "singletonExampleA";

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}