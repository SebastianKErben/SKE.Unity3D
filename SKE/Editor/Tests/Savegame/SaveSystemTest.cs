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

namespace SKE.Tests.Savegame {
    using System.Collections.Generic;

    using NUnit.Framework;
    using SKE.Unity3D.Savegame;

    public class SaveSystemTest {
        [Test]
        public void SaveDataTestBasic() {
            // Create test SaveData and fill it with values.
            var originalData = new TestSaveData();
            originalData.SaveText = "Hello world!";
            originalData.SaveNumber = 1337;

            // Serialize our test SaveData.
            var serializedData = originalData.Serialized;

            // Change the SaveExp value.
            originalData.SaveExp = 7f;

            // Create a SaveOperator.
            var saveOperator = new SaveOperator();

            // Use the SaveOperator to decode the serialized Json
            // data from before.
            var loadedData = saveOperator.DecodeData<TestSaveData>(serializedData);

            // The values of our test SaveData and the deserialized data should
            // be equal for SaveText and SaveNumber...
            Assert.AreEqual(originalData.SaveText, loadedData.SaveText);
            Assert.AreEqual(originalData.SaveNumber, loadedData.SaveNumber);

            // ...but not equal for SaveExp because we changed the value
            // after the serialization.
            Assert.AreNotEqual(originalData.SaveExp, loadedData.SaveExp);
        }

        [Test]
        public void SaveDataTestCrypto() {
            // Create test SaveData and fill the SaveText.
            var originalData = new TestSaveData();
            originalData.SaveText = "Hello world!";

            // Create a SaveOperator.
            var saveOperator = new SaveOperator();

            // Use the SaveOperator to encrpt encode the data.
            var serializedData = saveOperator.EncodeData(originalData, true);

            // Use the SaveOperator to decrypt decode the serialized data.
            var loadedData = saveOperator.DecodeData<TestSaveData>(serializedData, true);

            // The SaveText values of the original data and the decrypted
            // decoded serialized version should be equal.
            Assert.AreEqual(originalData.SaveText, loadedData.SaveText);
        }

        [Test]
        public void SaveTest() {
            // Create test SaveData and fill the SaveText.
            var originalData = new TestSaveData();
            originalData.SaveText = "Hello world!";

            // Create a SaveOperator and a test DataAccess and assign
            // the test SaveData and DataAccess to the SaveOperator to
            // emulate the SaveSystem singleton behaviour.
            var saveOperator = new SaveOperator();
            var saveAccess = new TestSaveDataAccess();
            saveOperator.DataAccess = saveAccess;
            saveOperator.CurrentSaveData = originalData;

            // Make sure the SaveData is saved as "First save".
            Assert.IsTrue(saveOperator.Save("First save"));

            // Change the SaveText of the savedata.
            originalData.SaveText = "Hello new world!";

            // Make sure the SaveData is saved as "Second save".
            Assert.IsTrue(saveOperator.Save("Second save"));

            // Make sure the SaveOperator will actually load the "First save".
            Assert.IsTrue(saveOperator.Load<TestSaveData>("First save"));

            // Since the Load method has created a new TestSaveData, we will
            // need to refresh the reference of originalData.
            originalData = saveOperator.GetLoadedSave<TestSaveData>();

            // Make sure the loaded SaveData SaveText value is equal to the
            // initial SaveText value.
            Assert.AreEqual("Hello world!", originalData.SaveText);

            // Make sure the SaveOperator will actually load the "Second save".
            Assert.IsTrue(saveOperator.Load<TestSaveData>("Second save"));

            // Since the Load method has created a new TestSaveData, we will
            // need to refresh the reference of originalData.
            originalData = saveOperator.GetLoadedSave<TestSaveData>();

            // Make sure the loaded SaveData SaveText value is equal to the
            // second SaveText value.
            Assert.AreEqual("Hello new world!", originalData.SaveText);
        }

        // Just a simple SaveData dataset for testing.
        class TestSaveData : SaveData {
            public string SaveText;
            public int SaveNumber;
            public float SaveExp;
        }

        // Just a simple SaveDataAccess for testing; it will save the
        // values in a dictionary.
        class TestSaveDataAccess : ISaveDataAccess {
            Dictionary<string, string> dataSet;

            public TestSaveDataAccess() {
                dataSet = new Dictionary<string, string>();
            }

            public string LoadData(string dataStore) {
                return dataSet.ContainsKey(dataStore) ? 
                    dataSet[dataStore] :
                    string.Empty;
            }

            public bool WriteData(string dataStore, string saveData) {
                dataSet[dataStore] = saveData;
                return true;
            }
        }
    }
}