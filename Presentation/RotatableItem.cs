using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Asterism {


	public static class SelectedItem {

		public static int holdCount;
		
	}

	public class RotatableItem : MonoBehaviour {

		[SerializeField]
		protected Transform target;

		[SerializeField]
		protected float rotationSpeed = 0.2f;

		[SerializeField]
		protected bool isTorus;

		private float innerRadius = 0.3f;

		private float outerRadius = 3f;

		public bool forceStop { get; set; }

		protected bool isClicked;
		private static int HoldCount {
			get => SelectedItem.holdCount;
			set => SelectedItem.holdCount = value;
		}

		private Camera cam;

		private void Start() {
			cam = Camera.main;

			this.LateUpdateAsObservable()
				.Where(_ => Input.GetMouseButtonDown(0))
				.Subscribe(_ => isClicked = IsClicked())
				.AddTo(this);

			this.LateUpdateAsObservable()
				.Where(_ => Input.GetMouseButton(0))
				.Subscribe(_ => MouseDrag())
				.AddTo(this);

			this.LateUpdateAsObservable()
				.Where(_ => Input.GetMouseButtonUp(0))
				.Subscribe(_ => {
					isClicked = false;
					HoldCount = Mathf.Max(HoldCount - 1, 0);
				})
				.AddTo(this);

			bool IsClicked() {
				if (forceStop) return false;
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
				if (isTorus && innerRadius > d) {
					return false;
				}

				bool isClick = d <= outerRadius;
				if (isClick) HoldCount++;
				return isClick;
			}
			
		}

		protected virtual void MouseDrag() {
			if (forceStop) return;
			if (!isClicked) return;

			float angle = Input.GetAxis("Mouse X") * rotationSpeed - Input.GetAxis("Mouse Y") * rotationSpeed;

			target.RotateAround(transform.position, Vector3.back, angle);
		}

	}

}