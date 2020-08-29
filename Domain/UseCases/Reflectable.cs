using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Asterism.Domain.UseCases {
	
	public interface IReflectable {

		BoolReactiveProperty Connecting { get; }
		void Reflect(Vector3 inDir, RaycastHit info);
		void Exit();
		int ReflectMax { get; }

	}

	public class Reflectable : MonoBehaviour, IReflectable {
		
		public BoolReactiveProperty Connecting { get; } = new BoolReactiveProperty();
		public int ReflectMax { get; } = DEFINE.REFLECT_MAX;

		private int refCount;

		private LineRenderer[] lineRenderers;
		
		private readonly Dictionary<LineRenderer, IReflectable> reflectableDictionary = new Dictionary<LineRenderer, IReflectable>();

		private void Awake() {

			lineRenderers = GetComponentsInChildren<LineRenderer>();
			Assert.IsTrue(lineRenderers.Length > 0);

			foreach (var v in lineRenderers) reflectableDictionary.Add(v, null);
		}

		private void Start() {
			this.LateUpdateAsObservable()
				.ThrottleFirstFrame(DEFINE.BUFFER_UPDATE_FRAME)
				.Subscribe(_ => LateUpdate())
				.AddTo(this);

			void LateUpdate() {

				// 複数反射しているときにどれかはずれた場合書き込みラインがずれてライン表示が重複するので毎回初期化
				// Exit()ほぼ要らなくなった気がする
				foreach (LineRenderer lineRenderer in lineRenderers.Skip(refCount)) {
					lineRenderer.positionCount = 0;
				}

				refCount = 0;
			}

		}

		public void Reflect(Vector3 inDir, RaycastHit info) {
			if (refCount >= ReflectMax) return;
			refCount++;
			
			var normal = info.normal;
			var hitPoint = info.point;

			var dir = Vector3.Reflect(inDir, normal);
			Ray ray = new Ray(hitPoint, dir);

			// Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);

			if (!Physics.Raycast(ray, out RaycastHit hit, DEFINE.RAY_MAX_DISTANCE, DEFINE.LASER_MASK)) return;

			var line = lineRenderers[refCount - 1];
				
			line.positionCount = 2;
			line.SetPosition(0, hitPoint);
			line.SetPosition(1, hit.point);

			var reflectable = reflectableDictionary[line];

			if (!hit.collider.CompareTag($"Refraction")) {
				reflectable?.Exit();
				reflectableDictionary[line] = null;
				return;
			}
				
			var tmp = hit.collider.GetComponent<IReflectable>();
			if (reflectable != null && reflectable != tmp) reflectable.Exit();
			reflectableDictionary[line] = tmp;
			reflectableDictionary[line].Reflect(ray.direction, hit);
		}

		public void Exit() {
			if (refCount >= ReflectMax) return;
			refCount++;

			foreach (var line in lineRenderers) {
				line.positionCount = 0;
				reflectableDictionary[line]?.Exit();
				reflectableDictionary[line] = null;
			}
		}
		
	}

}