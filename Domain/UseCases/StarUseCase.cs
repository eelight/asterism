using Asterism.Domain.UseCases;
using Doozy.Engine.UI;
using UniRx;
using UnityEngine;


namespace Asterism.Domain.UseCases {
	
	public interface IStarUseCase {

		BoolReactiveProperty Connecting { get; }
	}

	public class StarUseCase : MonoBehaviour, IStarUseCase {

		private ILineCheck lineCheck;

		[SerializeField]
		private GameObject[] connectingActiveObjects;
		
		[SerializeField]
		private GameObject[] connectingDisableObjects;

		[SerializeField]
		private UIButton seButton;

		// Start is called before the first frame update
		private void Awake() {

			lineCheck = GetComponent<ILineCheck>();

			Connecting
				.Subscribe(connect => {

					foreach (var v in connectingActiveObjects) {
						if(connect == v.activeSelf) continue;
						v.SetActive(connect);
					}

					foreach (var v in connectingDisableObjects) {
						if(!connect == v.activeSelf) continue;
						v.SetActive(!connect);
					}

					if (connect) {
						seButton.ExecuteClick();
					}

				}).AddTo(this);
		}

		public BoolReactiveProperty Connecting => lineCheck.IsHit;

	}

}