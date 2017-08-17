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

namespace SKE.Unity3D.Localization {
    using System;
    using System.Collections.Generic;

    using SKE.Unity3D.Singleton;
    using UnityEngine;

    /// <summary>
    /// Translator singleton.
    /// </summary>
    public class Translator : SingletonEternal<Translator> {
		string currentDictionary = "Dictionary";
		SystemLanguage currentLanguage;
		TranslationDictionaryOperator myOperator = new TranslationDictionaryOperator ();        

		/// <summary>
		/// Language changed handler.
		/// </summary>
        public delegate void LanguageChangedHandler ();

		/// <summary>
		/// Occurs when language changed.
		/// </summary>
        public static event LanguageChangedHandler LanguageChanged;

		/// <summary>
		/// Gets or sets the current dictionary.
		/// </summary>
		/// <value>The current dictionary.</value>
		public string CurrentDictionary {
			set {
				if (!value.Equals(currentDictionary)) {
					currentDictionary = value;

					myOperator.LoadTranslation(
						Resources.Load<TranslationDictionary>(CurrentDictionary));
				}
			}
			get {
				return currentDictionary;
			}
		}

        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>The current language.</value>
        public SystemLanguage CurrentLanguage {
            set {
                currentLanguage = value;                
				myOperator.currentLanguage = currentLanguage;

                if (LanguageChanged != null)
                    LanguageChanged();
            }
            get {
                return currentLanguage;
            }
        }

		/// <summary>
		/// Gets the translation.
		/// </summary>
		/// <returns>The translation.</returns>
		/// <param name="id">Identifier.</param>
		public string GetTranslation (string id) {
			return myOperator.GetTranslation(id);
		}

		/// <summary>
		/// Gets the translation.
		/// </summary>
		/// <returns>The translation.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="language">Language.</param>
		public string GetTranslation (string id, SystemLanguage language) {
			return myOperator.GetTranslation(id, language);
		}

        /// <summary>
        /// Will be called once the Singleton awakes the
        /// first time.
        /// </summary>
        public override void SingletonAwake() {
            CurrentLanguage = Application.systemLanguage;
            
			myOperator.LoadTranslation(
                Resources.Load<TranslationDictionary>(CurrentDictionary));
        }        
    }

    /// <summary>
    /// Translation dictionary operator.
    /// </summary>
    [Serializable]
    public class TranslationDictionaryOperator {
        /// <summary>
        /// The current language.
        /// </summary>
        public SystemLanguage currentLanguage;

        Dictionary<SystemLanguage, Dictionary<string, string>> content;

		/// <summary>
		/// Gets the translation.
		/// </summary>
		/// <returns>The translation.</returns>
		/// <param name="id">Identifier.</param>
		public string GetTranslation(string id) {
			return GetTranslation(id, currentLanguage);
		}

		/// <summary>
		/// Gets the translation.
		/// </summary>
		/// <returns>The translation.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="language">Language.</param>
		public string GetTranslation(string id, SystemLanguage language) {
			return content.ContainsKey(language) ? 
				content[language].ContainsKey(id) ? content[language][id] :
				string.Empty : string.Empty;
		}

        /// <summary>
        /// Loads the translation.
        /// </summary>
        /// <param name="dictionary">Dictionary.</param>
        public void LoadTranslation(TranslationDictionary dictionary) {
            if (dictionary == null)
                return;

            content = new Dictionary<SystemLanguage, Dictionary<string, string>>();

            foreach (var translation in dictionary.Translations) {
                content[translation.Language] = new Dictionary<string, string>();

                foreach (var entry in translation.Entry)
                    content[translation.Language][entry.Id] = entry.Content;
            }
        }        
    }
}