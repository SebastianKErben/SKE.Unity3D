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

	public class HighscoreTrackerTest {
		[Test]
		public void InitTracker() {
			// Create an operator instance.
			var testOperator = new HighscoreTrackerOperator ();

			// Test if it's there.
			Assert.IsNotNull (testOperator);
		}

		[Test]
		public void TrackTest() {
			// Create an operator instance.
			var testOperator = new HighscoreTrackerOperator ();

			// Score should be 0.
			Assert.AreEqual (0, testOperator.ScoreTotal);

			// Add 42 to the score.
			testOperator.Add (42);

			// Score should be 42.
			Assert.AreEqual (42, testOperator.ScoreTotal);

			// Add negative score.
			testOperator.Add (-42);

			// Score should still be 42.
			Assert.AreEqual (42, testOperator.ScoreTotal);
		}
	}
}