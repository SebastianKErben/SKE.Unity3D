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

namespace SKE.Unity3D.Analyze {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	using SKE.Unity3D.Singleton;
	using UnityEngine;

	/// <summary>
	/// Logging singleton.
	/// </summary>
	public class Logging : SingletonEternal<Logging> {
		readonly LoggingOperator myOperator = new LoggingOperator ();
		StreamWriter fileWriter;
		bool logfileOpen;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="SKE.Unity3D.Analyze.Logging"/> log singletong
		/// will also output the log entries to Debug.Log.
		/// </summary>
		/// <value><c>true</c> if log to Debug.Log; otherwise, <c>false</c>.</value>
		public bool LogToDebug {
			set;
			get;
		}

		/// <summary>
		/// Log the specified newEntry and toDebug.
		/// </summary>
		/// <param name="newEntry">New entry.</param>
		/// <param name="toDebug">If set to <c>true</c>, log will also go to Debug.Log.</param>
		public void Log(string newEntry, bool toDebug = false) {
			myOperator.Add (newEntry);

			if (LogToDebug || toDebug)
				Debug.Log (newEntry);

			if (!logfileOpen)
				StartCoroutine (LogToFile ());
		}

		/// <summary>
		/// Will be called once the Singleton awakes the
		/// first time.
		/// </summary>
		public override void SingletonAwake () {
			myOperator.Add ("Start writing to logfile...");
			StartCoroutine (LogToFile ());
		}

		IEnumerator LogToFile() {
			logfileOpen = true;

			var logFile = Path.Combine(
				Application.persistentDataPath, 
				DateTime.Now.ToLocalTime ().ToLongDateString () + "_log.txt");

			using (fileWriter = new StreamWriter(logFile, true)) {
				var nextEntry = myOperator.GetNext ();

				while (nextEntry != null) {
					fileWriter.WriteLine (nextEntry);
					yield return new WaitForSeconds (0.1f);
					nextEntry = myOperator.GetNext ();
				}					
			}

			logfileOpen = false;
		}

		void OnDestroy () {
			fileWriter.Close ();
		}
	}

	/// <summary>
	/// Logging operator class.
	/// </summary>
	[Serializable]
	public class LoggingOperator {
		readonly Queue<LoggingEntry> entries = new Queue<LoggingEntry> ();

		/// <summary>
		/// Add the specified newEntry to the log.
		/// </summary>
		/// <param name="newEntry">New entry.</param>
		public void Add(LoggingEntry newEntry) {
			entries.Enqueue (newEntry);
		}

		/// <summary>
		/// Add the specified newEntry to the log.
		/// </summary>
		/// <param name="newEntry">New entry.</param>
		public void Add(string newEntry) {
			entries.Enqueue (new LoggingEntry (newEntry));
		}

		/// <summary>
		/// Get the next log entry.
		/// </summary>
		/// <returns>The next.</returns>
		public LoggingEntry GetNext() {
			return entries.Count > 0 ? entries.Dequeue () : null;
		}
	}

	/// <summary>
	/// Logging entry class.
	/// </summary>
	[Serializable]
	public class LoggingEntry {
		/// <summary>
		/// The log entry time.
		/// </summary>
		public readonly DateTime logTime;

		/// <summary>
		/// The log entry.
		/// </summary>
		public readonly string logEntry;

		/// <summary>
		/// Initializes a new instance of the <see cref="SKE.Unity3D.Analyze.LoggingEntry"/> class.
		/// </summary>
		/// <param name="entry">Entry.</param>
		public LoggingEntry (string entry) {
			logTime = DateTime.Now.ToLocalTime ();
			logEntry = entry;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="SKE.Unity3D.Analyze.LoggingEntry"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="SKE.Unity3D.Analyze.LoggingEntry"/>.</returns>
		public override string ToString() {
			return logTime.ToLongTimeString () + " :: " + logEntry;
		}
	}
}