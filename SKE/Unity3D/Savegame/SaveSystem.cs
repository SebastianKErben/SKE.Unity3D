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

namespace SKE.Unity3D.Savegame {
    using System;
    using System.Text;

    using SKE.Unity3D.Cryptography;
    using SKE.Unity3D.Singleton;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Save system eternal singleton.
    /// </summary>
    public class SaveSystem : SingletonEternal<SaveSystem> {
        bool autosave;

        /// <summary>
        /// Gets or sets a value indicating whether this 
        /// <see cref="SKE.Unity3D.Savegame.SaveSystem"/> auto saves.
        /// </summary>
        /// <value><c>true</c> if auto save is active; 
        /// otherwise, <c>false</c>.</value>
        public bool AutoSave {
            set {
                if (autosave != value)
                    if (value)
                        SceneManager.activeSceneChanged += SaveAuto;
                    else
                        SceneManager.activeSceneChanged -= SaveAuto;

                autosave = value;
            }
            get {
                return autosave;
            }
        }

		/// <summary>
		/// Gets or sets the SaveOperators current SaveData.
		/// </summary>
		/// <value>The current save data.</value>
		public SaveData CurrentSaveData {
			set {
				if (CurrentSaveOperator != null)
					CurrentSaveOperator.CurrentSaveData = value;
			}
			get {
				return CurrentSaveOperator == null ? 
					null :
					CurrentSaveOperator.CurrentSaveData;
			}
		}

        /// <summary>
        /// Gets or sets the current SaveOperator.
        /// </summary>
        /// <value>The current save operator.</value>
        public SaveOperator CurrentSaveOperator {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets the SaveOperators DataAccess.
        /// </summary>
        /// <value>The data access.</value>
        public ISaveDataAccess DataAccess {
            set {
                if (CurrentSaveOperator != null)
                    CurrentSaveOperator.DataAccess = value;
            }
            get {
                return CurrentSaveOperator == null ?
                    null :
                    CurrentSaveOperator.DataAccess;
            }
        }
		
        /// <summary>
        /// Gets the SaveOperators current SaveData.
        /// </summary>
        /// <returns>The loaded save.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetLoadedSave<T>() where T : SaveData {
            return CurrentSaveOperator.CurrentSaveData as T;
        }

        /// <summary>
        /// Calls the Load method of the SaveOperator.
        /// </summary>
        /// <param name="saveId">Save identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public bool Load<T>(string saveId = "") where T : SaveData {
            return CurrentSaveOperator.Load<T>(saveId);
        }

        /// <summary>
        /// Calls the Save method of the SaveOperator.
        /// </summary>
        /// <param name="saveId">Save identifier.</param>
        public bool Save(string saveId = "") {
            return CurrentSaveOperator.Save(saveId);
        }

        /// <summary>
        /// Will be called once the Singleton awakes the
        /// first time.
        /// </summary>
        public override void SingletonAwake() {
            CurrentSaveOperator = new SaveOperator();
        }

        void OnApplicationFocus() {
            if (autosave)
                CurrentSaveOperator.Save("auto");
        }

        void OnApplicationPause() {
            if (autosave)
                CurrentSaveOperator.Save("auto");
        }

        void OnApplicationQuit() {
            if (autosave) {
                CurrentSaveOperator.Save("auto");
                SceneManager.activeSceneChanged -= SaveAuto;
            }
        }

        void SaveAuto(Scene previousScene, Scene nextScene) {
            if (autosave)
                CurrentSaveOperator.Save("auto");
        }
    }

	/// <summary>
	/// Save operator class.
	/// </summary>
    [Serializable]
    public class SaveOperator {
        string currentSave;
        DateTime currentSaveTime;

		/// <summary>
		/// Gets or sets the current SaveData identifier.
		/// </summary>
		/// <value>The current save.</value>
		public string CurrentSave {
			set {
				currentSave = value;
			}
			get {
				if (string.Empty.Equals(currentSave)) {
					currentSaveTime = DateTime.Now;
					currentSave = 
						Application.productName + currentSaveTime;
				}

				return currentSave;
			}
		}

		/// <summary>
		/// Gets or sets the current SaveData.
		/// </summary>
		/// <value>The current save data.</value>
		public SaveData CurrentSaveData {
			set;
			get;
		}

        /// <summary>
        /// Gets or sets the DataAccess.
        /// </summary>
        /// <value>The data access.</value>
        public ISaveDataAccess DataAccess {
            set;
            get;
        }

        /// <summary>
        /// Decodes the <paramref name="json"/> string into a
        /// <see cref="SaveData"/> object; optionally decrypts it via
        /// <see cref="Crypto"/>.
        /// </summary>
        /// <returns><see cref="SaveData"/> object.</returns>
        /// <param name="json">Json string.</param>
        /// <param name="decrypt">Optional, when true will attempt to decrypt
        /// with the optional <paramref name="salt"/>.</param>
        /// <param name="salt">Optional; will be used for decryption.</param>
        /// <typeparam name="T">Needs to be <see cref="SaveData"/> or inherit 
        /// from it.</typeparam>
        public T DecodeData<T>(string json, bool decrypt = false, string salt="") where T : SaveData {
            if (decrypt) {
                var encData = Convert.FromBase64String(json);
                var decData = Crypto.Decrypt(encData, salt);
                json = Encoding.UTF8.GetString(decData);
            }

            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Encodes the <paramref name="data"/> <see cref="SaveData"/> object;
        /// optionally encrypts it via <see cref="Crypto"/>.
        /// </summary>
        /// <returns><see cref="SaveData"/> object as Json string.</returns>
        /// <param name="data"><see cref="SaveData"/> object.</param>
        /// <param name="encrypt">Optional, when true will attempt to encrypt
        /// with the optional <paramref name="salt"/>.</param>
        /// <param name="salt">Optional; will be used for encryption.</param>
        /// <typeparam name="T">Needs to be <see cref="SaveData"/> or inherit 
        /// from it.</typeparam>
        public string EncodeData<T>(T data, bool encrypt = false, string salt="") where T : SaveData {
            return encrypt ? 
                Convert.ToBase64String(Crypto.Encrypt(data.Serialized, salt)) :
                data.Serialized;
        }

        /// <summary>
        /// Gets the currently active SaveData.
        /// </summary>
        /// <returns>The loaded save.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T GetLoadedSave<T>() where T : SaveData {
            return CurrentSaveData as T;
        }

        /// <summary>
        /// Load the specified SaveData as current SaveData.
        /// </summary>
        /// <param name="saveId">Save identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public bool Load<T>(string saveId = "") where T : SaveData {
            if (DataAccess == null)
                return false;

            if (string.Empty.Equals(saveId))
                saveId = CurrentSave;
            else
                CurrentSave = saveId;

            CurrentSaveData = DecodeData<T>(DataAccess.LoadData(saveId));

            return (CurrentSaveData != null);
        }

        /// <summary>
        /// Save the current SaveData via the current DataAccess with the
        /// specified saveId.
        /// </summary>
        /// <param name="saveId">Save identifier.</param>
        public bool Save(string saveId = "") {
            if (DataAccess == null || CurrentSaveData == null)
                return false;

            if (string.Empty.Equals(saveId))
                saveId = CurrentSave;
            else
                CurrentSave = saveId;
            
            return DataAccess.WriteData(saveId, EncodeData(CurrentSaveData));
        }
    }
}