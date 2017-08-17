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
     * Helper script for automatically updating the content
     * based on user interaction and the contentId.
     * Allows to change the contentId by sending a newId with
     * the UpdateText call.
    */
    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class TranslationUpdate : MonoBehaviour {
        [SerializeField]
        string contentId;
        UnityEngine.UI.Text myText;

		public void UpdateLanguage() {
			myText.text = Translator.Instance.GetTranslation(contentId);
		}

		public void UpdateText(string newId) {
			contentId = string.Empty.Equals(newId) ? contentId : newId;

			if (myText == null)
				myText = GetComponent<UnityEngine.UI.Text>();

			UpdateLanguage();
		}

		void OnDestroy() {
			Translator.LanguageChanged -= UpdateLanguage;
		}

        void Start() {
            myText = GetComponent<UnityEngine.UI.Text>();
            Translator.LanguageChanged += UpdateLanguage;
            UpdateLanguage();
        }        
    }
}