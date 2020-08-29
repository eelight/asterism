using System.Linq;
using Asterism.Domain.Entities;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asterism.Domain.UseCases {

	public interface IReferee {

		int MaxConnectCount { get; }
		IntReactiveProperty ConnectingCount { get; }
		Subject<Unit> IsStageClear { get; }
		int StageId { get; }
		
		IntReactiveProperty CubeConnectingCount { get; }
		IntReactiveProperty MaxCubeCount { get; }
		void OnAddCube();

	}

	public class Referee : MonoBehaviour, IReferee {

		public int MaxConnectCount { get; private set; }
		public IntReactiveProperty ConnectingCount { get; } = new IntReactiveProperty();
		public Subject<Unit> IsStageClear => stageClear.IsClear;
		public int StageId => stageLoader.GetStageId();
		public IntReactiveProperty CubeConnectingCount { get; } = new IntReactiveProperty();
		public IntReactiveProperty MaxCubeCount { get; } = new IntReactiveProperty();

		[Inject]
		private IStageLoader stageLoader;

		[Inject]
		private IStageClear stageClear;

		private readonly CompositeDisposable disposable = new CompositeDisposable();
		private float time;
		private bool isAllConnecting;
		private IVectorChanger[] vectorChangers;

		private void Start() {

			var stars = GetComponentsInChildren<IStarUseCase>();
			MaxConnectCount = stars.Length;

			stars.Select(s => s.Connecting).CombineLatest()
				.Do(e => ConnectingCount.Value = e.Count(l => l))
				.Do(list => isAllConnecting = list.All(l => l))
				.Subscribe()
				.AddTo(disposable, gameObject);
			
			vectorChangers = GameObject.Find("ObstacleRoot").GetComponentsInChildren<IVectorChanger>();
			MaxCubeCount.Value = vectorChangers.Count(v => !v.IsSleep);

			vectorChangers.Select(v => v.Connecting).CombineLatest()
						.Subscribe(list => CubeConnectingCount.Value = list.Count(l => l))
						.AddTo(disposable, gameObject);
		}

		private void Update() {
			if (!isAllConnecting) {
				time = 0f;
				return;
			}

			time += Time.deltaTime;
			if (time < DEFINE.STAGE_CLEAR_STAY_TIME) return;

			stageClear.Clear();
			disposable.Dispose();

			enabled = false;
			time = 0f;
		}

		private void OnDestroy() {
			if(!disposable.IsDisposed) disposable.Dispose();
		}

		public void OnAddCube() {
			var vectorChanger = vectorChangers.FirstOrDefault(v => v.IsSleep);
			if (vectorChanger == null) return;
			vectorChanger.Respawn();
			MaxCubeCount.Value++;
		}


	}

}