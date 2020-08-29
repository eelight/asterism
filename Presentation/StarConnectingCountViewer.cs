using Asterism.Domain.UseCases;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Asterism.Presentation {

	public class StarConnectingCountViewer : MonoBehaviour {

		[Inject]
		private ISaveData saveData;

		[Inject]
		private IReferee referee;

		[SerializeField]
		private TextMeshProUGUI textMeshPro;
		
		[SerializeField]
		private TextMeshProUGUI textMeshPro2;
		
		[SerializeField]
		private TextMeshProUGUI hiScore;

		// Start is called before the first frame update
		private async void Start() {

			await UniTask.DelayFrame(1);

			int maxCount = referee.MaxConnectCount;

			referee.ConnectingCount
					.Subscribe(c => textMeshPro.text = c + "/" + maxCount)
					.AddTo(this);

			textMeshPro.text = "0/" + maxCount;
			
			Observable.CombineLatest(referee.CubeConnectingCount, referee.MaxCubeCount)
					.Subscribe(list => textMeshPro2.text = "キューブ使用数 " + list[0] + "/" + list[1])
					.AddTo(this);

			int score = saveData.GetScore(referee.StageId);
			if (score >= 10) hiScore.text = "ハイスコア --";
			else hiScore.text = "ハイスコア " + score;

		}

		public void OnAddCube() {
			referee.OnAddCube();
		}

	}

}