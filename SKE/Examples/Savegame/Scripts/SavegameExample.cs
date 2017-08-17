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

namespace SKE.Examples.Savegame {
    using SKE.Unity3D.Savegame;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SavegameExample : MonoBehaviour {
    	// We want to store the references to our SaveSystem
        // and SaveData for easier access.
        SaveSystem mySaveSystem;
        SimpleSaveData mySaveData;

        void Awake () {
            // Get the reference to the SaveSystem instance
            mySaveSystem = SaveSystem.Instance;

            // If the SaveSystem doesn't already have a DataAccess
            // we will provide one.
            if (mySaveSystem.DataAccess == null)
                mySaveSystem.DataAccess = new DictionaryDataAccess();

            // If the SaveSystem doesn't have a SaveData dataset, we'll
            // create a new one;
            if (mySaveSystem.CurrentSaveData == null) {
                mySaveData = new SimpleSaveData();
                mySaveSystem.CurrentSaveData = mySaveData;
            }
            // if there is already a SaveData dataset, we'll get it.
            else
                mySaveData = mySaveSystem.GetLoadedSave<SimpleSaveData>();

            // Turn on AutoSave; we can still use the Save method to create
            // own saves.
            mySaveSystem.AutoSave = true;

            // Let's see where we're coming from; if this is the inital scene
            // the value will obviously be empty.
            Debug.Log("Coming from " + mySaveData.sceneName);

            // No tell our SaveData what the current scene is...
            mySaveData.sceneName = SceneManager.GetActiveScene().name;

            // ...and check if the value is also in our SaveSystem;
            // spoiler: it should be ;)
            Debug.Log("Now in " + mySaveSystem.GetLoadedSave<SimpleSaveData>().sceneName);
    	}

        void Update() {
            // Switch between both example scenes on any button press.
            if (Input.anyKeyDown) {
                var sceneToLoad = 
                    mySaveData.sceneName.Equals("savegameExampleA") ?
                    "savegameExampleB" : "savegameExampleA";

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // Our simple SaveData class, derived from SaveData.
    // Stores nothing more than a string representing the SceneName.
    public class SimpleSaveData : SaveData {
        public string sceneName;
    }
}