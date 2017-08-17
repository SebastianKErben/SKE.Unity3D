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

namespace SKE.Unity3D.Savegame.Utils {
	using System.IO;

	using UnityEngine;

	/// <summary>
	/// ISaveDataAccess implementation for file access.
	/// </summary>
	public class SaveFile : ISaveDataAccess {
		/// <summary>
		/// Loads data from a data store as Json.
		/// </summary>
		/// <returns>Save data serialized as json.</returns>
		/// <param name="dataStore">Data store.</param>
		public string LoadData(string dataStore) {
			var result = string.Empty;

			if (string.Empty.Equals (dataStore))
				return result;

			var filePath = Path.Combine (Application.persistentDataPath, dataStore);

			if (!File.Exists (filePath))
				return result;

			using (var fileReader = new StreamReader(filePath))
				result = fileReader.ReadToEnd ();

			return result;
		}

		/// <summary>
		/// Writes data to a data store.
		/// </summary>
		/// <returns><c>true</c>, if data was written, 
		/// <c>false</c> otherwise.</returns>
		/// <param name="dataStore">Data store.</param>
		/// <param name="saveData">Save data serialized as Json.</param>
		public bool WriteData(string dataStore, string saveData) {
			if (string.Empty.Equals (dataStore))
				return false;
			
			var filePath = Path.Combine (Application.persistentDataPath, dataStore);

			using (var fileWriter = new StreamWriter(filePath)) {
				fileWriter.Write (saveData);
				return true;
			}
		}
	}
}