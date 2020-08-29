using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Doozy.Engine.UI;
using naichilab;
using Prime31.TransitionKit;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Asterism.Domain.UseCases {

	public interface IStageResult {
		IObservable<Unit> IsStageClear { get; }
	}

	public class StageResult : MonoBehaviour, IStageResult {

		[Inject]
		private IReferee referee;

		[Inject]
		private ISaveData saveData;

		private TextAnimation textAnimation;

		[SerializeField]
		private GameObject clearDisableRoot;
		
		[SerializeField]
		private GameObject clearActiveRoot;

		[SerializeField]
		private GameObject[] clearActiveObjects;

		[SerializeField]
		private GameObject[] clearDisableObjects;

		[SerializeField]
		private UIButton seButton;

		private IFader[] disableFaders;
		private IFader[] activeFaders;
		private IVectorChanger[] vectorChangers;

		private void Awake() {
			disableFaders = clearDisableRoot.GetComponentsInChildren<IFader>();
			activeFaders = clearActiveRoot.GetComponentsInChildren<IFader>();
			vectorChangers = clearDisableRoot.GetComponentsInChildren<IVectorChanger>();
		}

		// Start is called before the first frame update
		private void Start() {

			referee.IsStageClear
						.Subscribe(_ => Clear().Forget())
						.AddTo(this);

			textAnimation = GetComponentInChildren<TextAnimation>();

		}

		private async UniTaskVoid Clear() {
			
			Debug.Log("clear");
			saveData.ClearStageId = referee.StageId;

			var score = vectorChangers.Count(v => v.Connecting.Value);
			saveData.SetScore(referee.StageId, score);

			foreach (var v in disableFaders) {
				v.FadeOut(1.5f);
			}

			foreach (var v in activeFaders) {
				v.FadeIn(1.5f);
			}

			foreach (var v in clearActiveObjects) v.SetActive(true);

			await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
			seButton.ExecuteClick();

			await textAnimation.StartEffectAsync();

			if (referee.StageId >= DEFINE.FINAL_STAGE_ID) {

				AllClear().Forget();
				return;
			}

			Transition();
			
		}

		protected virtual void Transition() {
			var wind = new WindTransition()
			{
				nextScene = DEFINE.MAIN,
				duration = 1.0f,
				size = 0.3f
			};
			TransitionKit.instance.transitionWithDelegate( wind );
		}

		private async UniTaskVoid AllClear() {
			Debug.Log("all clear");
			
			RankingLoader.Instance.SendScoreAndShowRanking(saveData.TotalScore);

			SceneManager.sceneUnloaded += ToTitle;

		}

		private void ToTitle(Scene scene) {
			if (!scene.name.Contains("Ranking")) return;
			
			SceneManager.sceneUnloaded -= ToTitle;
			var wind = new WindTransition()
			{
				nextScene = DEFINE.TITLE,
				duration = 1.0f,
				size = 0.3f
			};
			TransitionKit.instance.transitionWithDelegate( wind );
		}
		
		#if UNITY_EDITOR

		private void Update() {
			if(Input.GetKeyDown(KeyCode.T)) AllClear().Forget();
		}
		#endif

		public IObservable<Unit> IsStageClear => referee.IsStageClear;

	}

}