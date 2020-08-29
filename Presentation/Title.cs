using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Asterism;
using naichilab;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Title : MonoBehaviour {

	public static string[] playerNames;
	public static string playerOneName;

	[SerializeField]
	private NameAndStar nameAndStarPrefab;

	[SerializeField]
	private GameObject continueObject;

	[SerializeField]
	private GameObject[] endingActives;

	[SerializeField]
	private GameObject[] endingDisables;

	[SerializeField]
	private Transform playerStarTr;

	[SerializeField]
	private Transform[] otherPlayerTransforms;

	[Inject]
	private ISaveData saveData;

	[SerializeField]
	private GameObject graphController;

	[SerializeField]
	private GameObject[] tweetDisables;

	private bool loading;
	private static bool playedBgm;
	private static bool IsEnding => playerNames != null && playerNames.Length > 0;

	private void Awake() {

		continueObject.SetActive(!saveData.IsFirstPlay);

		if (!playedBgm) {
			graphController.SetActive(true);
			playedBgm = true;
		}

		if (IsEnding) {
			foreach (var v in endingActives) v.SetActive(true);
			foreach (var v in endingDisables) v.SetActive(false);

			CreatePlayerNameObject(gameObject.GetCancellationTokenOnDestroy()).Forget();
		}
	}

	private async UniTaskVoid CreatePlayerNameObject(CancellationToken token) {

		var ins = Instantiate(nameAndStarPrefab, playerStarTr, false);
		ins.SetName(playerOneName);

		var rnd = otherPlayerTransforms.Shuffle();

		int i = 0;
		foreach (var v in playerNames.Take(30)) {

			if (v == playerOneName) continue;

			try {
				var tr = rnd[i];
				var instance = Instantiate(nameAndStarPrefab, tr, false);
				instance.SetName(v);
				instance.SetRandomColor();

				if (i % 10 == 0) await UniTask.DelayFrame(1, cancellationToken: token);
				i++;
			} catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
		}

	}

	public void OnClickStart() {

		if (loading) return;

		ClickFirstStartAsync().Forget();

	}

	private async UniTaskVoid ClickFirstStartAsync() {

		saveData.ClearStageId = 0;

		ClickStartAsync().Forget();
	}

	public void OnClickContinue() {
		if (loading) return;
		ClickStartAsync().Forget();
	}

	private async UniTask ClickStartAsync() {
		loading = true;

		await SceneManager.LoadSceneAsync("Main");

		loading = false;
	}

	public void OnTweet() {
		TweetAsync().Forget();
	}

	private async UniTaskVoid TweetAsync() {
		foreach (var v in tweetDisables) v.SetActive(false);
		await UniTask.DelayFrame(1);
		UnityRoomTweet.TweetWithImage("asterism", "ALL STAGE CLEAR", "unity1week", "unityroom");
		await UniTask.DelayFrame(2);
		foreach (var v in tweetDisables) v.SetActive(true);
	}


}