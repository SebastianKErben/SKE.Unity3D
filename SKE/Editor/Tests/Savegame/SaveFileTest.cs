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
	using System.IO;

	using NUnit.Framework;
	using SKE.Unity3D.Savegame.Utils;
	using UnityEngine;

	public class SaveFileTest {
		[Test]
		public void SaveFileSimpleTest() {
			// Create a test string as data.
			const string testData = "Hello World";

			// Create a savefile instance and make sure it's not null.
			var testSave = new SaveFile ();
			Assert.IsNotNull (testSave);

			// Make sure we could write the data.
			Assert.IsTrue (testSave.WriteData ("test.sav", testData));

			// Make sure the written data loaded is the same as test data.
			Assert.AreEqual (testData, testSave.LoadData ("test.sav"));

			// Delete the file in the end.
			File.Delete (Path.Combine (Application.persistentDataPath, "test.sav"));
		}
	}
}