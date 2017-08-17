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

namespace SKE.Tests.Cryptography {
	using NUnit.Framework;
	using SKE.Unity3D.Cryptography.Utils;

	public class SecureStoreTest {
		[Test]
		public void InitOperator() {
			// Create a SecureStoreOperator to test.
			var testOperator = new SecureStoreOperator ();

			// test operator should not be null.
			Assert.IsNotNull (testOperator);
		}

		[Test]
		public void SimpleStoreGet() {
			// Create a SecureStoreOperator to test.
			var testOperator = new SecureStoreOperator ();

			// Store 42 under "test" and save the key.
			var testKey = testOperator.Store ("test", 42);

			// The test key should not be null.
			Assert.IsNotNull (testKey);

			// The value of the store should be 42.
			Assert.AreEqual (42, testOperator.Get ("test", testKey));

			// The value should now be 0 because it has been deleted through Get
			Assert.AreEqual (0, testOperator.Get ("test", testKey));
		}

		[Test]
		public void SimpleAdd() {
			// Create a SecureStoreOperator to test.
			var testOperator = new SecureStoreOperator ();

			// Store 22 under "testTwo" and save the key.
			var testKey = testOperator.Store ("testTwo", 22);

			// Add 20.
			testOperator.Add ("testTwo", testKey, 20);

			// The value of the store should be 42.
			Assert.AreEqual (42, testOperator.Get ("testTwo", testKey));
		}
	}
}