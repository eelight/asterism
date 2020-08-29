using Asterism.Presentation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asterism.Tutorial {

	public class TutorialDragableItem : DragableItem {

		[SerializeField]
		private Transform endPoint;

		[SerializeField]
		private float threshold = 1f;

		private bool isEnd;

		public bool IsTutorial { get; set; } = true;
		
		public async UniTask StartTutorial() {
			ForceStop = false;

			await UniTask.WaitUntil(() => isEnd);
		}

		protected override void MouseDrag() {
			base.MouseDrag();
			if (ForceStop) return;
			if (!IsTutorial) return;

			var dis = (transform.position - endPoint.position).sqrMagnitude;
			if (dis <= threshold) {

				ForceStop = true;

				transform.position = endPoint.position;
				transform.rotation = endPoint.rotation;
				isEnd = true;
			}
		}

	}

}