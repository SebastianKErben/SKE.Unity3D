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

namespace SKE.Tests.InputHelper {
    using NUnit.Framework;
    using SKE.Unity3D.InputHelper;
    using UnityEngine;

    // This test class for swiping.
    public class SwipingTest {

    	[Test]
    	public void SwipeOperatorTest() {
            // Create our variable for expected values and test it first time.
            var testExpected = Vector2.zero;
            Assert.AreEqual(testExpected.normalized, TestSwipe(ref testExpected));

            // Set expected to 1 and test it.
            testExpected = Vector2.one;
            Assert.AreEqual(testExpected.normalized, TestSwipe(ref testExpected));

            // Set expected to -1 and test it.
            testExpected = -Vector2.one;
            Assert.AreEqual(testExpected.normalized, TestSwipe(ref testExpected));
    	}

        Vector2 TestSwipe(ref Vector2 destination) {
            // Create a new SwipingOperator and register zero as start.
            var testOperator = new SwipingOperator();
            testOperator.RegisterStart(Vector3.zero);

            // Create endPoint from destination
            var endPoint = new Vector3(destination.x, destination.y);

            // Return the swipe vector.
            return testOperator.RegisterEnd(endPoint);
        }
    }
}