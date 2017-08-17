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

namespace SKE.Examples.Pooling {
    using System.Collections;

    using SKE.Unity3D.Pooling;
    using UnityEngine;

    // An PoolObject example that will fly in Direction with Speed for
    // Life.
    public class ExamplePoolObject : PoolObject<ExamplePoolObject> {
        public Vector3 direction;
		public float life;
		public float speed;

        float lifetime;

		IEnumerator Fly() {
			// Fly as long as the object didn't hit the max lifetime.
			while (lifetime < life) {
				transform.position += direction * speed * Time.deltaTime;
				yield return new WaitForEndOfFrame();
				lifetime += Time.deltaTime;
			}

			// Return to the pool.
			Return();
		}

        // Reset position and lifetime on deployment.
        void OnEnable() {
            transform.position = Vector3.zero;
            lifetime = 0f;

            // Start to fly.
            StartCoroutine(Fly());
        }        
    }
}