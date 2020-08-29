using System;
using Asterism.Domain.UseCases;
using UnityEngine;

namespace Asterism.Domain.UseCases {

	public interface IArrow {

		Vector3 Direction { get; }
		void SetLine(params Vector3[] points);
		bool Drawing { get; }
	}
}

namespace Asterism.Presentation {
	
	[RequireComponent(typeof(LineRenderer))]
	public class Arrow : MonoBehaviour, IArrow {

		private LineRenderer lineRenderer;

		private void Awake() {
			lineRenderer = GetComponent<LineRenderer>();
		}

		public Vector3 Direction => transform.up;

		public void SetLine(params Vector3[] points) {

			lineRenderer.positionCount = points.Length;

			for (int i = 0; i < points.Length; i++) {
				lineRenderer.SetPosition(i, points[i]);
			}
			
		}

		public bool Drawing => lineRenderer.positionCount > 1;

	}

}