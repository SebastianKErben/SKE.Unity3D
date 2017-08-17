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

namespace SKE.Tests.Localization {
    using System.Collections.Generic;

    using NUnit.Framework;
    using SKE.Unity3D.Localization;
    using UnityEngine;

    public class TranslatorTest {

    	[Test]
    	public void TranslationDictionaryTest() {
            // Can we create dictionaries?
            Assert.IsNotNull(CreateSimpleDictionary());
    	}

        [Test]
        public void TranslatorOperatorTest() {
            // Create the test dictionary.
            var testDictionary = CreateSimpleDictionary();

            // Can we create a TranslationDictionaryOperator?
            var testOperator = new TranslationDictionaryOperator();
            Assert.IsNotNull(testOperator);

            // Load our test dictionary in the operator and set the
            // language to "en".
            testOperator.LoadTranslation(testDictionary);
            testOperator.currentLanguage = SystemLanguage.English;

            // The text for "hello" should be "Hello World!" for
            // "en" but not for "de".
            Assert.AreEqual("Hello World!", testOperator.GetTranslation("hello"));
            Assert.AreNotEqual("Hello World!",
                testOperator.GetTranslation("hello", SystemLanguage.German));

            // Now we switch the language to "de".
            testOperator.currentLanguage = SystemLanguage.German;

            // The text for "hello" should be "Hallo Welt!" for
            // "de" but not for "en".
            Assert.AreEqual("Hallo Welt!", testOperator.GetTranslation("hello"));
            Assert.AreNotEqual("Hallo Welt!",
                testOperator.GetTranslation("hello", SystemLanguage.English));
        }

        /// <summary>
        /// Creates a simple TranslationDictionary for testing.
        /// </summary>
        /// <returns>The simple dictionary.</returns>
        public TranslationDictionary CreateSimpleDictionary() {
            /* Our simple test dictionary will contain 2 
             * translations for "English and German of 
             * 1 entry "hello".
             * For English the translation will be "Hello World!",
             * for German the translation will be "Hallo Welt!".
            */
            var testEntryA = new TranslationEntry();
            testEntryA.Id = "hello";
            testEntryA.Content = "Hello World!";

            var testEntryB = new TranslationEntry();
            testEntryB.Id = "hello";
            testEntryB.Content = "Hallo Welt!";

            var testTranslationA = new Translation();
            testTranslationA.Language = SystemLanguage.English;
            testTranslationA.Entry = new List<TranslationEntry>();
            testTranslationA.Entry.Add(testEntryA);

            var testTranslationB = new Translation();
            testTranslationB.Language = SystemLanguage.German;
            testTranslationB.Entry = new List<TranslationEntry>();
            testTranslationB.Entry.Add(testEntryB);

            var testDictionary = ScriptableObject.CreateInstance<TranslationDictionary>();
            testDictionary.Translations = new List<Translation>();
            testDictionary.Translations.Add(testTranslationA);
            testDictionary.Translations.Add(testTranslationB);

            return testDictionary;
        }
    }
}