using Asterism.Domain.UseCases;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Asterism.Presentation {

	public class LineRendererFader : MonoBehaviour, IFader {

		[SerializeField]
		private LineRenderer[] lineRenderers;

		[SerializeField]
		private bool manualSet;

		[ContextMenu("Set")]
		private void Set() {
			lineRenderers = GetComponentsInChildren<LineRenderer>(true);
		}

		private void Awake() {
			if(!manualSet) lineRenderers = GetComponentsInChildren<LineRenderer>();
		}

		public async UniTask FadeIn(float duration) {

			Tweener tween = null;
			foreach (var lineRenderer in lineRenderers) {

				var start = lineRenderer.startColor;
				start.a = 0f;
				Color end = start;
				end.a = 1f;

				tween = lineRenderer.DOColor(new Color2(start, start), new Color2(end, end), duration);
			}

			if (tween == null) await UniTask.DelayFrame(1);
			else await tween;
		}

		public async UniTask FadeOut(float duration) {
		
			Tweener tween = null;
			foreach (var lineRenderer in lineRenderers) {

				var start = lineRenderer.startColor;
				Color end = start;
				end.a = 0f;

				tween = lineRenderer.DOColor(new Color2(start, start), new Color2(end, end), duration);
			}

			if (tween == null) await UniTask.DelayFrame(1);
			else await tween;
		}

	}

}