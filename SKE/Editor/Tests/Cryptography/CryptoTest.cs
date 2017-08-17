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
    using System;

    using NUnit.Framework;
    using SKE.Unity3D.Cryptography;

    public class CryptoTest {

        [Test]
        public void Integer() {
            var originalData = Crypto.RandomHash;

            // Initial test without explicit salt.
            var encryptedData = Crypto.Encrypt(originalData);
            var decryptedData = Crypto.DecryptInt32(encryptedData);

            Assert.AreEqual(originalData, decryptedData);

            // Second test with explicit salt.
            var salt = Crypto.RandomHash.ToString();
            encryptedData = Crypto.Encrypt(originalData, salt);
            decryptedData = Crypto.DecryptInt32(encryptedData, salt);

            Assert.AreEqual(originalData, decryptedData);
        }

        [Test]
        public void String() {
            var originalData = Crypto.RandomHash.ToString();

            // Initial test without explicit salt.
            var encryptedData = Crypto.Encrypt(originalData);
            var decryptedData = Crypto.DecryptUTF8String(encryptedData);

            Assert.AreEqual(originalData, decryptedData);

            // Second test with explicit salt.
            var salt = Crypto.RandomHash.ToString();
            encryptedData = Crypto.Encrypt(originalData, salt);
            decryptedData = Crypto.DecryptUTF8String(encryptedData, salt);

            Assert.AreEqual(originalData, decryptedData);
        }
    }
}