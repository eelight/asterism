using System.Linq;
using Asterism.Domain.UseCases;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Asterism.Domain.UseCases {

	public interface ILineCheck {

		BoolReactiveProperty IsHit { get; }

	}

}

namespace Asterism.Presentation {


	public class LineCheck : MonoBehaviour, ILineCheck {

		private LineRenderer[] lineRenderers;

		public BoolReactiveProperty IsHit { get; } = new BoolReactiveProperty();

		private float radius = 0.033f;

		private void Start() {

			lineRenderers = FindObjectsOfType<LineRenderer>().Where(e => e.gameObject.layer != 1 << 1).ToArray();

			this.UpdateAsObservable()
				.ThrottleFirstFrame(DEFINE.BUFFER_UPDATE_FRAME)
				.Subscribe(_ => Update())
				.AddTo(this);

			void Update() {

				bool isHit = false;
				foreach (var lineRenderer in lineRenderers) {
					if (lineRenderer.positionCount < 2) {
						continue;
					}

					var pos1 = lineRenderer.GetPosition(0);
					var pos2 = lineRenderer.GetPosition(1);

					var center = transform.position;
					var x = center.PerpendicularFootPoint(pos1, pos2);

					bool outOfLine = Mathf.Max((x - pos1).sqrMagnitude, (x - pos2).sqrMagnitude) > (pos1 - pos2).sqrMagnitude;
					if (outOfLine) {
						continue;
					}


					var dis = (center - x).sqrMagnitude;
					isHit = dis <= radius;
					if (isHit) break;
				}

				if (IsHit.Value != isHit) IsHit.Value = isHit;

			}

		}


	}

}