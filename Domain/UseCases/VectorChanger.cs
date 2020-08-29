using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Asterism.Domain.UseCases {

	public interface IVectorChanger : IReflectable {

		void Respawn();
		bool IsSleep { get; }

	}
	
	public class VectorChanger : MonoBehaviour, IVectorChanger {

		public BoolReactiveProperty Connecting { get; } = new BoolReactiveProperty();
		public int ReflectMax { get; } = DEFINE.REFLECT_MAX;

		private int updateCount;

		private IArrow[] arrows;

		private readonly Dictionary<IArrow, IReflectable> reflectableDictionary = new Dictionary<IArrow, IReflectable>();

		[SerializeField]
		private bool isSleep;

		[SerializeField]
		private Vector3 respawnPoint = new Vector3(-3f, 3.75f, 0f);

		private void Awake() {

			IsSleep = isSleep;

			arrows = GetComponentsInChildren<IArrow>();
			Assert.IsTrue(arrows.Length > 0);

			foreach (var v in arrows) reflectableDictionary.Add(v, null);
		}
		
		private void Start() {
			this.LateUpdateAsObservable()
				.ThrottleFirstFrame(DEFINE.BUFFER_UPDATE_FRAME)
				.Subscribe(_ => LateUpdate())
				.AddTo(this);

			void LateUpdate() {

				Connecting.Value = arrows.All(a => a.Drawing);
				
				updateCount = 0;
			}

		}

		public void Reflect(Vector3 inDir, RaycastHit info) {
			if (updateCount >= ReflectMax) return;
			updateCount++;
			
			var transform1 = transform;

			foreach (var arrow in arrows) {
				var reflectable = reflectableDictionary[arrow];
				Ray ray = new Ray(transform1.position, arrow.Direction);
				
				// Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);

				if (!Physics.Raycast(ray, out RaycastHit hit, DEFINE.RAY_MAX_DISTANCE, DEFINE.LASER_MASK)) return;

				arrow.SetLine(transform1.position, hit.point);
				
				if (!hit.collider.CompareTag($"Refraction")) {
					reflectable?.Exit();
					reflectableDictionary[arrow] = null;
					continue;
				}
				
				var tmp = hit.collider.GetComponent<IReflectable>();
				if (reflectable != null && reflectable != tmp) reflectable.Exit();
				reflectableDictionary[arrow] = tmp;
				reflectableDictionary[arrow].Reflect(ray.direction, hit);
			}
			
		}

		public void Exit() {
			if (updateCount >= ReflectMax) return;
			updateCount++;

			foreach (var v in arrows) {
				v.SetLine();
				reflectableDictionary[v]?.Exit();
				reflectableDictionary[v] = null;
			}

		}

		public void Respawn() {
			if (!IsSleep) return;
			IsSleep = false;
			
			transform.DOMove(respawnPoint, 1f);
			
		}

		/// <summary>
		/// 外部からAwakeで呼ぶの禁止
		/// </summary>
		public bool IsSleep { get; private set; }

	}

}