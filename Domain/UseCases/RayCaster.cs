using Asterism.Domain.UseCases;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Asterism.Domain.UseCases {

	public interface IRayCaster {

	}

	[RequireComponent(typeof(LineRenderer))]
	public class RayCaster : MonoBehaviour, IRayCaster {

		private LineRenderer lineRenderer;
		
		private Transform trCache;

		private IReflectable reflectable;

		// Start is called before the first frame update
		private void Awake() {
			lineRenderer = GetComponent<LineRenderer>();
			trCache = transform;
		}

		private void Start() {

			this.UpdateAsObservable()
				.ThrottleFirstFrame(DEFINE.BUFFER_UPDATE_FRAME)
				.Subscribe(_ => Update())
				.AddTo(this);

			void Update() {
				
				Ray ray = new Ray(trCache.position, trCache.up);

				if (!Physics.Raycast(ray, out RaycastHit hit, DEFINE.RAY_MAX_DISTANCE, DEFINE.LASER_MASK)) return;
				lineRenderer.positionCount = 2;
				lineRenderer.SetPosition(0, trCache.position);
				lineRenderer.SetPosition(1, hit.point);

				if (!hit.collider.CompareTag($"Refraction")) {
					reflectable?.Exit();
					reflectable = null;
					return;
				}
				
				var tmp = hit.collider.GetComponent<IReflectable>();
				if (reflectable != null && reflectable != tmp) reflectable.Exit();
				reflectable = tmp;
				reflectable.Reflect(ray.direction, hit);
			}
		}


	}

}