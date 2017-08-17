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

namespace SKE.Unity3D.Singleton {
    using UnityEngine;

    /// <summary>
    /// Abstract singleton eternal class. As the name implies, the singleton 
    /// will survive scene switches.
    /// </summary>
    public abstract class SingletonEternal<T> : MonoBehaviour where T : Component {
        /// <summary>
        /// Gets the instance of this SingletonBehaviour; 
        /// creates one if no one exists.
        /// </summary>
        /// <value>SingletonBehaviour instance.</value>
        public static T Instance {
            get {
                if (Lock == null)
                    Lock = new object();
                
                lock (Lock) {
                    if (SingletonInstance == null) {
                        var singletonInstance = new GameObject();
                        singletonInstance.name = typeof(T).Name;
                        SingletonInstance = singletonInstance.AddComponent<T>();
                    }

                    return SingletonInstance;
                }
            }
        }

		/// <summary>
		/// Gets or sets the lock.
		/// </summary>
		/// <value>The lock.</value>
        static object Lock {
            set;
            get;
        }

		/// <summary>
		/// Gets or sets the singleton instance.
		/// </summary>
		/// <value>The singleton instance.</value>
        static T SingletonInstance {
            set;
            get;
        }

		/// <summary>
		/// Will be called once the Singleton awakes the
		/// first time.
		/// </summary>
		public virtual void SingletonAwake() {
		}

        void Awake() {
            if (SingletonInstance == null) {
                SingletonInstance = this as T;
                DontDestroyOnLoad(this);
                SingletonAwake();
            }
            else if (SingletonInstance != this)
                Destroy(gameObject);
        }        
    }
}