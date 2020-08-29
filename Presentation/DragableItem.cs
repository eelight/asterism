using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Asterism.Presentation {

	public class DragableItem : MonoBehaviour {
		
		public bool ForceStop { get; set; }

		private Camera cam;
		
		private float myRadius = .25f;

		private bool isClicked;
		private static int HoldCount {
			get => SelectedItem.holdCount;
			set => SelectedItem.holdCount = value;
		}
		
		private void Start() {
			cam = Camera.main;
			
			this.UpdateAsObservable()
				.Where(_ => Input.GetMouseButtonDown(0))
				.Subscribe(_ => isClicked = IsClicked())
				.AddTo(this);

			this.UpdateAsObservable()
				.Where(_ => Input.GetMouseButton(0))
				.Subscribe(_ => MouseDrag())
				.AddTo(this);
			
			this.UpdateAsObservable()
				.Where(_ => Input.GetMouseButtonUp(0))
				.Subscribe(_ => {
					isClicked = false;
					HoldCount = Mathf.Max(HoldCount - 1, 0);
				})
				.AddTo(this);

			bool IsClicked() {
				if (ForceStop) return false;
				if (HoldCount >= 1) return false;
				
				var position = transform.position;
				Vector3 objectPointInScreen = cam.WorldToScreenPoint(position);

				Vector3 mousePointInScreen
					= new Vector3(Input.mousePosition.x,
						Input.mousePosition.y,
						objectPointInScreen.z);

				Vector3 mousePointInWorld = cam.ScreenToWorldPoint(mousePointInScreen);
				mousePointInWorld.z = position.z;

				var d = (mousePointInWorld - position).sqrMagnitude;
				bool isClick = d <= myRadius;
				if (isClick) HoldCount++;
				return isClick;
			}

		}

		protected virtual void MouseDrag() {
			if (ForceStop) return;
			if (!isClicked) return;
				
			var position = transform.position;
			Vector3 objectPointInScreen = cam.WorldToScreenPoint(position);

			Vector3 mousePointInScreen
				= new Vector3(Input.mousePosition.x,
					Input.mousePosition.y,
					objectPointInScreen.z);

			Vector3 mousePointInWorld = cam.ScreenToWorldPoint(mousePointInScreen);
			mousePointInWorld.z = position.z;
			position = mousePointInWorld;
			transform.position = position;
		}

	}

}