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
    using System.Collections.Generic;

    using SKE.Unity3D.Savegame;

    // We create our own DataAccess class based on ISaveDataAccess;
    // it can be anything (FileAccess, CloudSaving, ...) but to keep it 
    // simple this will just be a simple Dictionary.
    public class DictionaryDataAccess : ISaveDataAccess {
        Dictionary<string, string> dataSet;

        public DictionaryDataAccess() {
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