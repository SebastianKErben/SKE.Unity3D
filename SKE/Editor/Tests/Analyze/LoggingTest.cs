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

namespace SKE.Tests.Analyze {
	using NUnit.Framework;
	using SKE.Unity3D.Analyze;

	public class LoggingTest {
		[Test]
		public void LoggingEntryTest() {
			// Create a simple log entry.
			var testEntry = new LoggingEntry ("just a test");

			// Make sure it's not null.
			Assert.IsNotNull (testEntry);

			// Make sure the log entry is the same as used to create it.
			Assert.AreEqual ("just a test", testEntry.logEntry);

			// Make sure the DateTime is not null.
			Assert.IsNotNull (testEntry.logTime);
		}

		[Test]
		public void LoggingOperatorTest() {
			// Create a test operator.
			var testOperator = new LoggingOperator();

			// Make sure it's not null.
			Assert.IsNotNull(testOperator);

			// Add an example entry.
			testOperator.Add("hello world");

			// Get the next entry in the operators log.
			var testEntry = testOperator.GetNext();

			// The entry shouldn't be null.
			Assert.IsNotNull(testEntry);

			// The entry should be the "hello world".
			Assert.AreEqual("hello world", testEntry.logEntry);
		}
	}
}