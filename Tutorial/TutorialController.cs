using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Asterism.Tutorial {

	public class TutorialController : MonoBehaviour {

		[Inject]
		private ISaveData saveData;

		[SerializeField]
		private TextMeshProUGUI textMeshPro;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[FormerlySerializedAs("moveMirror")]
		[SerializeField]
		private GameObject popReflectionCube;

		[FormerlySerializedAs("moveMirrorEndTransform")]
		[SerializeField]
		private Transform popCubeEndPoint;

		[SerializeField]
		private TutorialRotatableItem tutorialRotatableTriangleItem;
		
		[SerializeField]
		private TutorialRotatableItem tutorialRotatableCubeItem;

		[SerializeField]
		private TutorialDragableItem tutorialDragableItem;

		[SerializeField]
		private TutorialStageResult tutorialStageResult;

		[SerializeField]
		private CanvasGroup moveCanvasGroup;

		[SerializeField]
		private CanvasGroup rotateCanvasGroup1;

		[SerializeField]
		private CanvasGroup rotateCanvasGroup2;

		[SerializeField]
		private Button[] disableButtons;

		[SerializeField]
		private CanvasGroup buttonFocusCanvasGroup;

		private int moveCubeId;
		private int rotateCubeId;
		private int rotateTriangleId;
		private float fadeTime = 0.3f;

		public bool IsTutorial { get; private set; } = true;

		private void Start() {
			StartAsync(gameObject.GetCancellationTokenOnDestroy());

			foreach (var v in disableButtons) v.interactable = false;
		}

		private async void StartAsync(CancellationToken token) {
			
			UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

			textMeshPro.text = "チュートリアルを開始します。";
			// textMeshPro.text = $"ようこそ、{saveData.PlayerName}さん!\r\nこのゲームを遊んでくれて感謝します!";

			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);

			if (saveData.TutorialCount > 0) {

				textMeshPro.text = $"ここで合うのは{saveData.TutorialCount+1}回目ですね!";

				await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			}

			textMeshPro.text = "このゲームの目的は光を反射させてすべての星を光らせることです。";
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			
			textMeshPro.text = "キューブをドラッグして指定ポイントに移動させてください。";
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);

			moveCanvasGroup.DOFade(1f, fadeTime);

			canvasGroup.DOFade(0f, fadeTime);
			await tutorialDragableItem.StartTutorial();
			canvasGroup.DOFade(1f, fadeTime);

			moveCanvasGroup.DOFade(0f, fadeTime);

			textMeshPro.text = "グッド! そのままキューブ周囲をなぞって回転させましょう。\r\n3つ光る箇所を探してみてください。";
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			rotateCanvasGroup1.DOFade(1f, fadeTime);
			await tutorialRotatableCubeItem.StartTutorial();

			textMeshPro.text = "すばらしい! 次に三角を回転させて4つ光らせましょう。";
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			rotateCanvasGroup1.DOFade(0f, fadeTime);
			rotateCanvasGroup2.DOFade(1f, fadeTime);
			canvasGroup.DOFade(0f, fadeTime);
			
			await tutorialRotatableTriangleItem.StartTutorial();
			
			canvasGroup.DOFade(1f, fadeTime);
			rotateCanvasGroup2.DOFade(0f, fadeTime);
			
			textMeshPro.text = "あと1つ星を光らせればクリアですが現状だと難しそうです。\r\nこのようなときは新しいキューブを追加しましょう";
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			
			textMeshPro.text = "こちらのボタンをクリックしてください";

			foreach (var b in disableButtons) b.interactable = true;
			buttonFocusCanvasGroup.DOFade(1f, fadeTime);

			await UniTask.WaitUntil(() => !isCubeAddWaiting, cancellationToken: token);
			
			buttonFocusCanvasGroup.DOFade(0f, fadeTime);
			textMeshPro.text = "いつでもキューブは補給できます。では残りの星を光らせてみてください";
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			
			tutorialDragableItem.ForceStop = false;
			tutorialDragableItem.IsTutorial = false;
			tutorialRotatableCubeItem.EndTutorial();
			tutorialRotatableTriangleItem.EndTutorial();
			// tutorialStageResult.IsTutorial = false;
			IsTutorial = false;
			
			bool waiting = true;
			tutorialStageResult.IsStageClear
								.First()
								.Subscribe(_ => waiting = false)
								.AddTo(this);
			
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			canvasGroup.DOFade(0f, fadeTime);

			await UniTask.WaitUntil(() => !waiting, cancellationToken: token);

			canvasGroup.DOFade(1f, fadeTime);
			textMeshPro.text = "おめでとう!!クリアしました。";
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			
			textMeshPro.text = "このステージはいつでもタイトルから開始することができます。";
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);
			
			textMeshPro.text = "ステージは残り4つです。最後までクリアするとちょっとだけいいことがあります\r\nぜひクリアを目指してみてください。";
			await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return), cancellationToken: token);

			saveData.TutorialCount++;
			
			tutorialStageResult.TutorialEndTransition();

		}

		public void TutorialEnd() {
			canvasGroup.DOFade(0f, fadeTime);

			tutorialDragableItem.ForceStop = false;
			tutorialDragableItem.IsTutorial = false;
			tutorialRotatableCubeItem.EndTutorial();
			tutorialRotatableTriangleItem.EndTutorial();
			tutorialStageResult.IsTutorial = false;
			IsTutorial = false;
			Destroy(gameObject);

		}

		private bool isCubeAddWaiting = true;
		
		public void OnAddCube() {
			AddCubeDelay().Forget();
		}

		private async UniTaskVoid AddCubeDelay() {
			await UniTask.Delay(TimeSpan.FromSeconds(1f));
			isCubeAddWaiting = false;
		}

	}

}