using System;
using System.IO;
using UnityEngine;

namespace Asterism {

	public interface ISaveData {

		int ClearStageId { get; set; }
		string PlayerName { get; set; }
		bool IsFirstPlay { get; }
		int TutorialCount { get; set; }

		int TotalScore { get; }
		void SetScore(int stageId, int score);
		int GetScore(int stageId);

	}

	public class SaveData : ISaveData {

		public int ClearStageId {
			get => PlayerPrefs.GetInt("ClearStageId", 0);
			set {
				PlayerPrefs.SetInt("ClearStageId", value);
				PlayerPrefs.Save();
			}
		}

		public string PlayerName {
			get => PlayerPrefs.GetString("PlayerName", "Nanashi");
			set {
				PlayerPrefs.SetString("PlayerName", value);
				PlayerPrefs.Save();
			}
		}

		public bool IsFirstPlay { get; }

		public int TutorialCount {
			get => PlayerPrefs.GetInt("TutorialCount", 0);
			set {
				PlayerPrefs.SetInt("TutorialCount", value);
				PlayerPrefs.Save();
			}
		}

		public int Stage1HighScore {
			get => PlayerPrefs.GetInt("Stage1HighScore", 10);
			set {
				if (Stage1HighScore <= value) return;
				PlayerPrefs.SetInt("Stage1HighScore", value);
				PlayerPrefs.Save();
			}
		}

		public int Stage2HighScore {
			get => PlayerPrefs.GetInt("Stage2HighScore", 10);
			set {
				if (Stage2HighScore <= value) return;
				PlayerPrefs.SetInt("Stage2HighScore", value);
				PlayerPrefs.Save();
			}
		}

		public int Stage3HighScore {
			get => PlayerPrefs.GetInt("Stage3HighScore", 10);
			set {
				if (Stage3HighScore <= value) return;
				PlayerPrefs.SetInt("Stage3HighScore", value);
				PlayerPrefs.Save();
			}
		}

		public int Stage4HighScore {
			get => PlayerPrefs.GetInt("Stage4HighScore", 10);
			set {
				if (Stage4HighScore <= value) return;
				PlayerPrefs.SetInt("Stage4HighScore", value);
				PlayerPrefs.Save();
			}
		}

		public int Stage5HighScore {
			get => PlayerPrefs.GetInt("Stage5HighScore", 10);
			set {
				if (Stage5HighScore <= value) return;
				PlayerPrefs.SetInt("Stage5HighScore", value);
				PlayerPrefs.Save();
			}
		}

		public int TotalScore => Stage1HighScore + Stage2HighScore + Stage3HighScore + Stage4HighScore + Stage5HighScore;

		public void SetScore(int stageId, int score) {
			switch (stageId) {
				case 1:
					Stage1HighScore = score;
					break;
				case 2:
					Stage2HighScore = score;
					break;
				case 3:
					Stage3HighScore = score;
					break;
				case 4:
					Stage4HighScore = score;
					break;
				case 5:
					Stage5HighScore = score;
					break;
			}
		}

		public int GetScore(int stageId) {
			int score = 10;
			switch (stageId) {
				case 1:
					 score = Stage1HighScore;
					break;
				case 2:
					 score = Stage2HighScore;
					break;
				case 3:
					 score = Stage3HighScore;
					break;
				case 4:
					 score = Stage4HighScore;
					break;
				case 5:
					 score = Stage5HighScore;
					break;
			}

			return score;
		}

		public void Reset() {
			PlayerPrefs.DeleteAll();
		}

		public SaveData() {
			IsFirstPlay = ClearStageId <= 0;
		}

	}

}