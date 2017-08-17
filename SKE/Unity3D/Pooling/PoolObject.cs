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

namespace SKE.Unity3D.Pooling {
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Abstract self managing pool object.
    /// </summary>
    public abstract class PoolObject<T> : MonoBehaviour where T : Component {
        /// <summary>
        /// The prefab reference.
        /// </summary>
        public GameObject prefabReference;

		/// <summary>
		/// Gets or sets the pool.
		/// </summary>
		/// <value>The pool.</value>
		public static Queue<PoolObject<T>> Pool {
			set;
			get;
		}

		/// <summary>
		/// Gets or sets the storage transform.
		/// </summary>
		/// <value>The storage.</value>
		public static Transform Storage {
			set;
			get;
		}

        /// <summary>
        /// Clear the specified amount of objects from the pool.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public static int Clear(int amount = 0) {
            lock (Pool) {
                amount = amount < 1 ? Pool.Count : 
                    amount > Pool.Count ? Pool.Count : amount;

                PoolObject<T> clearObject;


                for (var iterator = 0; iterator != amount; ++iterator) {
                    clearObject = Pool.Dequeue();

                    if (clearObject != null)
                        Destroy(clearObject.gameObject);
                }
            }

            return amount;
        }

        /// <summary>
        /// Gets an instance of this pool object. If <paramref name="createNew"/>
        /// is set to <c>true</c> or the pool is empty, a new object will be 
        /// instantiated.
        /// </summary>
        /// <returns>A GameObject instance.</returns>
        /// <param name="createNew">If set to <c>true</c> force to create new.</param>
        public GameObject GetInstance(bool createNew = false) {
            if (Storage == null) {
                Storage = new GameObject().transform;
				Storage.name = prefabReference.name;
            }

            if (Pool == null)
                Pool = new Queue<PoolObject<T>>();

            lock (Pool) {
                if (!createNew && Pool.Count > 0) {
                    PoolObject<T> returnObject = null;

                    while (returnObject == null) {
                        try {
                            returnObject = Pool.Dequeue();
                        }
                        catch {
                            returnObject = null;
                        }
                    }

                    returnObject.transform.parent = null;
                    returnObject.gameObject.SetActive(true);

                    return returnObject.gameObject;
                }
                else
					return Instantiate<GameObject>(prefabReference);
            }
        }

        /// <summary>
        /// Ready the specified amount of GameObjects in the pool.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public bool Ready(int amount) {
            if (amount < 1)
                return false;

            for (var iterator = 0; iterator != amount; ++iterator)
                GetInstance(true).GetComponent<PoolObject<T>>().Return();

            return true;
        }

        /// <summary>
        /// Return this instance to the pool.
        /// </summary>
        public void Return() {
            lock (Pool)
                Pool.Enqueue(this);
            
            transform.parent = Storage;
            gameObject.SetActive(false);
        }
    }
}