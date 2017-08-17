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
	/// Highscore tracker singleton.
	/// </summary>
	public class HighscoreTracker : SingletonEternal<HighscoreTracker> {
		HighscoreTrackerOperator myOperator;

		/// <summary>
		/// Gets the current total score.
		/// </summary>
		/// <value>The score total.</value>
		public int ScoreTotal {
			get {
				return myOperator.ScoreTotal;
			}
		}

		/// <summary>
		/// Add the specified score.
		/// </summary>
		/// <param name="score">Score.</param>
		public int Add (int score = 0) {
			return myOperator.Add (score);
		}

		/// <summary>
		/// Will be called once the Singleton awakes the
		/// first time.
		/// </summary>
		public override void SingletonAwake () {
			myOperator = new HighscoreTrackerOperator ();
		}
	}

	/// <summary>
	/// Highscore tracker operator class.
	/// </summary>
	[Serializable]
	public class HighscoreTrackerOperator {
		int[] scoreHash = { 0, 0, 0, 0 };
		Dictionary<int, int> scoreParts = new Dictionary<int, int>();
		int scoreTotal;

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="SKE.Unity3D.Cryptography.Utils.HighscoreTrackerOperator"/> class.
		/// </summary>
		public HighscoreTrackerOperator() {
			scoreTotal = 0;

			var tempValue = Crypto.RandomHash;

			for (var iterator = 0; iterator != scoreHash.Length; ++iterator) {
				scoreHash [iterator] = Crypto.RandomHash;

				if (iterator % 2 == 0)
					scoreParts.Add (scoreHash [iterator], tempValue);
				else
					scoreParts.Add (scoreHash [iterator], -tempValue);
			}

			tempValue = Crypto.RandomHash;

			for (var iterator = 0; iterator != scoreHash.Length; ++iterator) {
				if (iterator < 2)
					scoreParts [scoreHash [iterator]] += tempValue;
				else
					scoreParts [scoreHash [iterator]] -= tempValue;
			}
		}

		/// <summary>
		/// Add the specified score.
		/// </summary>
		/// <param name="score">Score.</param>
		public int Add (int score = 1) {
			if (score < 1)
				return ScoreTotal;

			if (int.MaxValue - score < scoreTotal)
				return int.MaxValue;

			scoreTotal += score;

			for (var iterator = 0; iterator != scoreHash.Length; ++iterator)
				if (iterator < 2)
					scoreParts [scoreHash [iterator]] += score;
				else
					scoreParts [scoreHash [iterator]] -= score;

			scoreParts [scoreHash [UnityEngine.Random.Range (0, scoreHash.Length)]] += score;

			return ScoreTotal;
		}

		/// <summary>
		/// Gets the total score.
		/// </summary>
		/// <value>The score total.</value>
		public int ScoreTotal {
			get {
				var hashTotal = 0;

				for (var iterator = 0; iterator != scoreHash.Length; ++iterator)
					hashTotal += scoreParts [scoreHash [iterator]];

				if (scoreTotal != hashTotal)
					scoreTotal = hashTotal;

				return scoreTotal < 0 ? int.MaxValue : scoreTotal;
			}
		}
	}
}