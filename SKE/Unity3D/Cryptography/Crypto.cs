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

namespace SKE.Unity3D.Cryptography {
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using UnityEngine;

    /// <summary>
    /// Static crypto class.
    /// </summary>
    public static class Crypto {
        /// <summary>
        /// Gets a random hash int.
        /// </summary>
        /// <value>Random hash int.</value>
        public static int RandomHash {
            get {
                return Guid.NewGuid().GetHashCode();
            }
        }

		/// <summary>
		/// Decrypt the specified encryptedData, optionally with a given salt.
		/// </summary>
		/// <param name="encryptedData">Encrypted data.</param>
		/// <param name="salt">Optional salt string; if not set, defaults
		/// to the company name like the Encrypt method.</param>
		public static byte[] Decrypt(byte[] encryptedData, string salt = "") {
			salt = string.Empty.Equals(salt) ? 
				Application.companyName.Replace(" ", string.Empty) : 
				salt;

			try {
				var pepper = BitConverter.ToString(
					MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt)));

				var inVector = Encoding.ASCII.GetBytes(pepper.Substring(0, 32));
				var pass = new StringBuilder();
				var appName = Application.productName.Replace(" ", string.Empty);

				for (var iter = 0; iter != appName.Length; ++iter)
					if ((iter % 2) == 0)
						pass.Append(appName[iter]);

				var password = new Rfc2898DeriveBytes(
					pass.ToString(), 
					Encoding.UTF8.GetBytes(pepper.Substring(16, 16)));

				var keyBytes = password.GetBytes(32);

				using (var symmetricKey = new RijndaelManaged()) {
					symmetricKey.Mode = CipherMode.CBC;
					symmetricKey.BlockSize = 256;

					using (var decryptor = symmetricKey.CreateDecryptor(
						keyBytes, inVector)) {

						using (var memoryStream = new MemoryStream(encryptedData)) {
							using (var cryptoStream = new CryptoStream(
								memoryStream, decryptor, CryptoStreamMode.Read)) {

								var decrypted = new byte[encryptedData.Length];
								cryptoStream.Read(decrypted, 0, decrypted.Length);

								memoryStream.Close();
								cryptoStream.Close();

								return decrypted;
							}
						}
					}
				}
			}
			catch {
				return null;
			}
		}

		/// <summary>
		/// Decrypts encrypted data to Int32.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="encryptedData">Encrypted data.</param>
		/// <param name="salt">Salt.</param>
		public static int DecryptInt32(byte[] encryptedData, string salt = "") {
			return BitConverter.ToInt32(Decrypt(encryptedData, salt), 0);
		}

		/// <summary>
		/// Decrypts encrypted data to UTF8 string.
		/// </summary>
		/// <returns>The UTF8 string.</returns>
		/// <param name="encryptedData">Encrypted data.</param>
		/// <param name="salt">Salt.</param>
		public static string DecryptUTF8String(byte[] encryptedData, string salt = "") {

			return Encoding.UTF8.GetString(Decrypt(encryptedData, salt)).Trim('\0');
		}

        /// <summary>
        /// Encrypt the specified rawData, optionally with a given salt.
        /// </summary>
        /// <param name="rawData">Raw byte data.</param>
        /// <param name="salt">Optional salt string; if not set the salt
        /// will be the company name of the application.</param>
        public static byte[] Encrypt(byte[] rawData, string salt = "") {
            salt = string.Empty.Equals(salt) ? 
                Application.companyName.Replace(" ", string.Empty) : 
                salt;

            var pepper = BitConverter.ToString(
                MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(salt)));

            var inVector = Encoding.UTF8.GetBytes(pepper.Substring(0, 32));
            var pass = new StringBuilder();
            var appName = Application.productName.Replace(" ", string.Empty);

            for (var iter = 0; iter != appName.Length; ++iter)
                if (iter % 2 == 0)
                    pass.Append(appName[iter]);

            var password = new Rfc2898DeriveBytes(
                pass.ToString(), 
                Encoding.UTF8.GetBytes(pepper.Substring(16, 16)));

            var keyBytes = password.GetBytes(32);

            using (var symmetricKey = new RijndaelManaged()) {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.BlockSize = 256;

                using (var encryptor = symmetricKey.CreateEncryptor(
                    keyBytes, inVector)) {

                    using (var memoryStream = new MemoryStream()) {
                        using (var cryptoStream = new CryptoStream(
                            memoryStream, encryptor, CryptoStreamMode.Write)) {

                            cryptoStream.Write(rawData, 0, rawData.Length);
                            cryptoStream.FlushFinalBlock();

                            var encrypted = memoryStream.ToArray();

                            memoryStream.Close();
                            cryptoStream.Close();
                            return encrypted;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypt the specified int rawData.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        /// <param name="salt">Salt.</param>
        public static byte[] Encrypt(int rawData, string salt = "") {
            return Encrypt(BitConverter.GetBytes(rawData), salt);
        }

        /// <summary>
        /// Encrypt the specified string rawData.
        /// </summary>
        /// <param name="rawData">Raw data.</param>
        /// <param name="salt">Salt.</param>
        public static byte[] Encrypt(string rawData, string salt = "") {
            return Encrypt(Encoding.UTF8.GetBytes(rawData), salt);
        }        
    }
}