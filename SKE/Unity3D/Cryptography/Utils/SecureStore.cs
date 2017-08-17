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

namespace SKE.Unity3D.Cryptography.Utils {
	using System;
	using System.Collections.Generic;

	using SKE.Unity3D.Singleton;

	/// <summary>
	/// Secure store singleton.
	/// </summary>
	public class SecureStore : SingletonEternal<SecureStore> {
		SecureStoreOperator myOperator = new SecureStoreOperator();

		/// <summary>
		/// Add the specified value to the entry belonging to name and key combination.
		/// </summary>
		/// <returns>The updated value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public int Add(string name, int key, int value) {
			return myOperator.Add (name, key, value);
		}

		/// <summary>
		/// Get the value belonging to the specified name and key. The value will be deleted from the 
		/// secure store.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="key">Key.</param>
		public int Get(string name, int key) {
			return myOperator.Get (name, key);
		}

		/// <summary>
		/// Store the specified value under the given name.
		/// </summary>
		/// <returns>The key for the stored value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public int Store(string name, int value) {
			return myOperator.Store (name, value);
		}
	}

	/// <summary>
	/// Secure store operator class.
	/// </summary>
	[Serializable]
	public class SecureStoreOperator {
		Dictionary<string, byte[]> valueStore = new Dictionary<string, byte[]>();

		/// <summary>
		/// Add the specified value to the entry belonging to name and key combination.
		/// </summary>
		/// <returns>The updated value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public int Add (string name, int key, int value) {
			if (!valueStore.ContainsKey (name))
				return 0;

			var oldValue = Get (name, key);
			oldValue += value;

			var newValue = Crypto.Encrypt (oldValue, key.ToString ());
			valueStore [name] = newValue;

			return oldValue;
		}

		/// <summary>
		/// Get the value belonging to the specified name and key. The value will be deleted from the 
		/// secure store.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="key">Key.</param>
		public int Get (string name, int key) {
			if (!valueStore.ContainsKey (name))
				return 0;
			
			var returnValue = Crypto.DecryptInt32 (valueStore [name], key.ToString ());
			valueStore.Remove (name);

			return returnValue;
		}

		/// <summary>
		/// Store the specified value under the given name.
		/// </summary>
		/// <returns>The key for the stored value.</returns>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public int Store (string name, int value) {
			var sKey = Crypto.RandomHash;
			var toStore = Crypto.Encrypt (value, sKey.ToString ());

			valueStore [name] = toStore;

			return sKey;
		}
	}
}