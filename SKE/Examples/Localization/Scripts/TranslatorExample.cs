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

namespace SKE.Examples.Localization {
    using SKE.Unity3D.Localization;
    using UnityEngine;

    /* 
     * See the ExampleDictionary file in the example's resource folder.
     * You can create your own Dictionary by right-clicking in the Project
     * folder list -> selecting Create -> chose SKE -> Dictionary.
    */
    public class TranslatorExample : MonoBehaviour {
        [SerializeField]
        UnityEngine.UI.Text phraseField;
        string phraseId;

		// Switch beteen German and English.
		public void ChangeLanguage() {
			Translator.Instance.CurrentLanguage = 
				Translator.Instance.CurrentLanguage.Equals(SystemLanguage.English) ?
				SystemLanguage.German :
				SystemLanguage.English;
		}

		// Switch between "hello" and "bye" for the phrase.
		public void ChangePhrase() {
			phraseId = "hello".Equals(phraseId) ? "bye" : "hello";
			phraseField.GetComponent<TranslationUpdate>().UpdateText(phraseId);
		}

        /*
         * Change the dictionary from the default "Dictionary" to
         * "ExampleDictionary", set the language to English and
         * update the phrase text because it will still say "New Text"
         * at the beginning.
        */
        void Awake () {
            Translator.Instance.CurrentDictionary = "ExampleDictionary";
            Translator.Instance.CurrentLanguage = SystemLanguage.English;
            ChangePhrase();
        }        
    }
}