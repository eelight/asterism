using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Asterism {

	public class NameAndStar : MonoBehaviour {

		[SerializeField]
		private TextMeshProUGUI text;

		public void SetName(string n) {
			text.text = n;

			var c = text.color;
			c.a = 0f;
			text.color = c;

			text.DOFade(1f, Random.Range(duration.x, duration.y));
		}

		[SerializeField, Range(0f, 1f)]
		private float hueMin;

		[SerializeField, Range(0f, 1f)]
		private float hueMax = 1f;

		[SerializeField, Range(0f, 1f)]
		private float saturationMin;

		[SerializeField, Range(0f, 1f)]
		private float saturationMax = 1f;

		[SerializeField]
		private Renderer particle;

		private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

		[SerializeField]
		private Vector2 intensity;

		[SerializeField]
		private Vector2 duration = new Vector2(0.3f, 0.5f);

		[SerializeField]
		private Renderer[] renderers;

		public void SetRandomColor() {
			var color = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, 1f, 1f);

			var mat = particle.material;
			mat.EnableKeyword("_EMISSION");

			color = color * Random.Range(intensity.x, intensity.y);
			mat.SetColor(EmissionColor, color);

			var materials = renderers.Select(r => r.material).ToArray();

			foreach (var material in materials) {
				var c1 = material.GetColor(BaseColor);
				c1.a = 0f;
				material.SetColor(BaseColor, c1);				
			}

			foreach (var material in materials) {
				var c2 = material.GetColor(BaseColor);
				c2.a = 1f;
				DOTween.To(() => material.GetColor(BaseColor), x => material.SetColor(BaseColor, x), c2, Random.Range(duration.x, duration.y));	
			}
			

		}

	}

}